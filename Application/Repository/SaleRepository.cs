using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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

        try{
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync(); 

            var saleCreated = await _context.Sales
                                    .Where(u=> u.Id == newSale.Id)
                                    .FirstOrDefaultAsync();
            var medicine = await _context.Medicines
                                        .Where(u=> u.Id == modelSaleMedicine.MedicineId)
                                        .FirstOrDefaultAsync();
            var newSaleMedicine = new SaleMedicine
            {
                SaleId = saleCreated.Id,
                MedicineId = modelSaleMedicine.MedicineId,
                SaleQuantity= modelSaleMedicine.SaleQuantity,
                Price = medicine.Price*modelSaleMedicine.SaleQuantity
            };
            try{
                
                if (medicine.Stock >= newSaleMedicine.SaleQuantity)
                {
                    medicine.Stock -= modelSaleMedicine.SaleQuantity;
                
                    _context.SaleMedicines.Add(newSaleMedicine);
                    _context.Medicines.Update(medicine);
                    await _context.SaveChangesAsync();
                }else{
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);
                    await _context.SaveChangesAsync();
                    return "No hay tantos medicamentos";

                }
            }catch(Exception ex){
                return $"{ex.Message}. Details: {ex.Data}";
            } 
        }catch(Exception ex)
        {
            return $"{ex.Message}. Details: {ex.Data}";
        }
        
        return "Sale made successfully!!";
        
        
       
    }
    public async Task<string> RegisterManyMedicinesAsync(Sale modelSale,  List<SaleMedicine> list)
    {
        var newSale = new Sale
        {
            DateSale = DateTime.UtcNow,
            PatientId = modelSale.PatientId,
            EmployeeId = modelSale.EmployeeId,
            Prescription = modelSale.Prescription
        };

        try{
            _context.Sales.Add(newSale);
            await _context.SaveChangesAsync();

            var saleCreated = await _context.Sales
                                        .Where(u=> u.Id == newSale.Id)
                                        .FirstOrDefaultAsync();

            List<SaleMedicine> newSaleMedicines = new(); 

            foreach(var saleMedicine in list)
            {
                var medicine = await _context.Medicines
                                    .Where(u=>u.Id == saleMedicine.MedicineId)
                                    .FirstOrDefaultAsync();
                
                
                newSaleMedicines.Add(new SaleMedicine
                {
                    SaleId = saleCreated.Id,
                    MedicineId = saleMedicine.MedicineId,
                    SaleQuantity= saleMedicine.SaleQuantity,
                    Price = medicine.Price*saleMedicine.SaleQuantity,
                });

                if (medicine.Stock >= saleMedicine.SaleQuantity)
                {
                    medicine.Stock -= saleMedicine.SaleQuantity;
                    
                    _context.Medicines.Update(medicine);
                    await _context.SaveChangesAsync();
                }else{
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("medicamento creado");
                    return "No hay tantos medicamentos";
                }
            } 
            try{
                _context.SaleMedicines.AddRange(newSaleMedicines);
                await _context.SaveChangesAsync();

            }catch(Exception ex){
                return ex.Message;
            }
        }catch(Exception ex)
        {
            return ex.Message;
        }
        
        return "Sale made successfully!!";
    }
    public async Task<IEnumerable<Sale>> GetAllRecipesAsync()
    {
        DateTime sice2023 = new(2023,1,1);
        return await _context.Sales.Where(m => m.Prescription == true && m.DateSale >= sice2023).ToListAsync();
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

    //Promedio de medicamentos comprados por venta
    // public async Task<IEnumerable<object>> GetAverage()
    // {
    //     var sales = await _context.Sales.ToListAsync();

    //     var promedioVentasPorVenta = sales.Select(sale => new
    //     {
            
    //         SaleId = sale.Id,
    //         PromedioVenta = sale.SaleMedicines.Average(sm => sm.SaleQuantity)
    //     });

    //     var prom = sales
    //             .SelectMany(sale => sale.SaleMedicines, (sale, medicine) => new
    //             {
    //                 SaleId = sale.Id,
    //                 MedicineId = medicine.MedicineId,
    //                 PromedioVenta = sale.SaleMedicines.Average(sm => sm.SaleQuantity)
    //             });
    //     return prom;
    // }
}
