using System.Collections;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class SaleRepository : GenericRepository<Sale>, ISale
{
    private readonly PharmacyDbContext _context;

    public SaleRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<string> RegisterAsync(Sale modelSale, SaleMedicine modelSaleMedicine)
    {
        var newSale = new Sale
        {
            DateSale = DateTime.UtcNow,
            PatientId = modelSale.PatientId,
            EmployeeId = modelSale.EmployeeId,
            Prescription = modelSale.Prescription
        };

        try
        {
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync();

            var saleCreated = await _context.Sales
                                    .Where(u => u.Id == newSale.Id)
                                    .FirstOrDefaultAsync();

            var medicine = await _context.Medicines
                                        .Where(u => u.Id == modelSaleMedicine.MedicineId)
                                        .FirstOrDefaultAsync();

            var newSaleMedicine = new SaleMedicine
            {
                SaleId = saleCreated.Id,
                MedicineId = modelSaleMedicine.MedicineId,
                SaleQuantity = modelSaleMedicine.SaleQuantity,
                Price = medicine.Price * modelSaleMedicine.SaleQuantity
            };

            try
            {
                var lot = await _context.PurchasedMedicines
                                        .Where(u => u.MedicineId == medicine.Id)
                                        .FirstOrDefaultAsync();

                if (medicine.Stock >= newSaleMedicine.SaleQuantity)
                {
                    medicine.Stock -= modelSaleMedicine.SaleQuantity;
                    lot.Stock -= modelSaleMedicine.SaleQuantity;

                    _context.SaleMedicines.Add(newSaleMedicine);
                    _context.Medicines.Update(medicine);
                    _context.PurchasedMedicines.Update(lot);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);

                    await _context.SaveChangesAsync();
                    return "No hay tantos medicamentos";

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "Sale made successfully!!";
    }

    public async Task<string> RegisterManyMedicinesAsync(Sale modelSale, List<SaleMedicine> list)
    {
        var newSale = new Sale
        {
            DateSale = DateTime.UtcNow,
            PatientId = modelSale.PatientId,
            EmployeeId = modelSale.EmployeeId,
            Prescription = modelSale.Prescription
        };

        try
        {
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync();

            var saleCreated = await _context.Sales
                                        .Where(u => u.Id == newSale.Id)
                                        .FirstOrDefaultAsync();

            List<SaleMedicine> newSaleMedicines = new();

            foreach (var saleMedicine in list)
            {
                var medicine = await _context.Medicines
                                    .Where(u => u.Id == saleMedicine.MedicineId)
                                    .FirstOrDefaultAsync();


                newSaleMedicines.Add(new SaleMedicine
                {
                    SaleId = saleCreated.Id,
                    MedicineId = saleMedicine.MedicineId,
                    SaleQuantity = saleMedicine.SaleQuantity,
                    Price = medicine.Price * saleMedicine.SaleQuantity,
                });

                var lot = await _context.PurchasedMedicines
                                        .Where(u => u.MedicineId == medicine.Id)
                                        .FirstOrDefaultAsync();

                if (medicine.Stock >= saleMedicine.SaleQuantity)
                {
                    medicine.Stock -= saleMedicine.SaleQuantity;
                    // lot.Stock -= saleMedicine.SaleQuantity;

                    _context.Medicines.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);
                    await _context.SaveChangesAsync();
                    return "No hay tantos medicamentos";
                }
            }
            try
            {
                _context.SaleMedicines.AddRange(newSaleMedicines);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "Sale made successfully!!";
    }
    public async Task<IEnumerable<Sale>> GetAllRecipesAsync()
    {
        DateTime sice2023 = new(2023, 1, 1);
        return await _context.Sales.Where(m => m.Prescription == true && m.DatePrescription >= sice2023).ToListAsync();
    }
    public async Task<IEnumerable<Sale>> GetSaleMonthly(int parameter)
    {
        DateTime date = new(2023, parameter, 1);
        var query = _context.Sales as IQueryable<Sale>;
        var registros = await query
                                .Where(p => p.DateSale.Year == date.Year && p.DateSale.Month == date.Month)
                                .ToListAsync();
        return registros;
    }

    // Promedio de medicamentos comprados por venta
    public async Task<object> GetAverage()
    {
        var sales = await _context.Sales.ToListAsync();

//         var promedioVentasPorVenta = Enumerable.Empty<object>();

// if (sales != null && sales.Any())
// {
//     promedioVentasPorVenta = sales.Select(sale => new
//     {
//         SaleId = sale.Id,
//         PromedioVenta = sale.SaleMedicines.Average(sm => sm.SaleQuantity)
//     });
// }
        // var promedioVentasPorVenta = sales.Select(sale => new
        // {
        //     SaleId = sale.Id,
        //     PromedioVenta = sale.SaleMedicines.Average(sm => sm.SaleQuantity)
        // });

        var prom = sales
    .Where(sale => sale != null && sale.SaleMedicines != null)
    .SelectMany(sale => sale.SaleMedicines, (sale, medicine) => new
    {
        SaleId = sale.Id,
        MedicineId = medicine.MedicineId,
        SaleQuantity = medicine.SaleQuantity
    })
    .GroupBy(item => new { item.SaleId, item.MedicineId })
    .Select(group => new
    {
        SaleId = group.Key.SaleId,
        MedicineId = group.Key.MedicineId,
        PromedioVenta = group.Average(item => item.SaleQuantity)
    });

        return prom;
    }

    // Cantidad de ventas realizadas por cada empleado en 2023
    public async Task<object> GetSaleQuantityAsync()
    {

        var employees = await _context.Employees.ToListAsync();  
        var quantity = employees.Select(u=> new {u.Name, u.Sales.Count});

        return quantity;
    }
}
