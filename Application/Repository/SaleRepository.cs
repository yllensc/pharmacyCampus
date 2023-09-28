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

                if (medicine.Stock >= newSaleMedicine.SaleQuantity)
                {
                    int quantity =  modelSaleMedicine.SaleQuantity;
                    
                    while (quantity != 0)
                    {
                        var nearestExpirationDate =await _context.PurchasedMedicines
                                                    .Where(u=> u.MedicineId ==  modelSaleMedicine.MedicineId && u.Stock>0)
                                                    .OrderBy(o=> o.ExpirationDate).FirstAsync();
                        if(nearestExpirationDate == null)
                        {
                            quantity = 0;
                        }else{
                            var purchasedMedicine = await _context.PurchasedMedicines.Where(u => u.Id == nearestExpirationDate.Id).FirstOrDefaultAsync();

                            if(nearestExpirationDate.Stock>= quantity)
                            {
                                //Stock Lote por fecha de vencimiento
                                purchasedMedicine.Stock -= quantity;
                                                                Console.WriteLine(purchasedMedicine.Stock +"dfgdfg  " +quantity +"  fdgdsfg");

                                _context.PurchasedMedicines.Update(purchasedMedicine);
                                await _context.SaveChangesAsync();
                                quantity = 0;

                            }else
                            {
                                quantity -=  purchasedMedicine.Stock ;
                                purchasedMedicine.Stock =0;
                                _context.PurchasedMedicines.Update(purchasedMedicine);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }


                    medicine.Stock -= modelSaleMedicine.SaleQuantity;

                    _context.SaleMedicines.Add(newSaleMedicine);
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
            catch (Exception ex)
            {
                return $"{ex.Message}. Details: {ex.Data}";
            }
        }
        catch (Exception ex)
        {
            return $"{ex.Message}. Details: {ex.Data}";
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

                if (medicine.Stock >= saleMedicine.SaleQuantity)
                {
                    medicine.Stock -= saleMedicine.SaleQuantity;

                    _context.Medicines.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Sales.Remove(newSale);
                    _context.Sales.Remove(saleCreated);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("medicamento creado");
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
    public async Task<object> GetTotalSalesOneMedicine(string nameMedicine)
    {
        var existMedicine = await _context.Medicines
                                    .Where(u => u.Name.ToLower() == nameMedicine.ToLower())
                                    .FirstOrDefaultAsync();
        if (existMedicine == null)
        {
            return null;
        }

        var salesMedicine = await _context.SaleMedicines
                                    .Where(u => u.MedicineId == existMedicine.Id)
                                    .ToListAsync();

        object totalSales = new
        {
            TotalSales = salesMedicine.Count
        };
        return totalSales;
    }

    public async Task<object> GetGainSales()
    {
        var totalGain = await _context.SaleMedicines.SumAsync(u => u.Price);

        object totalSales = new
        {
            TotalSales = totalGain
        };
        return totalSales;
    }

    public async Task<IEnumerable<Medicine>> GetUnsoldMedicine()
    {
        var medicines = await _context.Medicines.ToListAsync();

        List<Medicine> unsoldMed = new();
        foreach (var med in medicines)
        {
            var existMed = await _context.SaleMedicines
                                        .Where(u => u.MedicineId == med.Id)
                                        .FirstOrDefaultAsync();
            if (existMed == null)
            {
                unsoldMed.Add(med);
            }
        }
        return unsoldMed;
    }

    public async Task<IEnumerable<Medicine>> GetUnsoldMedicines2023()
    {
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();

        List<Medicine> unsoldMed = new();
        foreach (var sale in sales)
        {
            var listSales = await _context.SaleMedicines
                                        .Where(u => u.SaleId == sale.Id)
                                        .ToListAsync();
            foreach (var med in medicines)
            {
                var existMed = listSales
                                    .Where(u => u.MedicineId == med.Id)
                                    .FirstOrDefault();
                if (existMed == null && !unsoldMed.Contains(med))
                {
                    unsoldMed.Add(med);
                }
            }
        }
        return unsoldMed;
    }

    public async Task<IEnumerable<Patient>> GetPatients(string nameMedicine)
    {
        var existMedicine = await _context.Medicines
                                    .Where(u => u.Name.ToLower() == nameMedicine.ToLower())
                                    .FirstOrDefaultAsync();
        if (existMedicine == null)
        {
            return null;
        }
        var patients = await _context.Patients.ToListAsync();
        var sales = await _context.Sales.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();

        var patientsPurchasedMedicine = (from patient in patients
                                         join sale in sales on patient.Id equals sale.PatientId
                                         join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                                         join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                                         where medicine.Name == nameMedicine
                                         select patient).Distinct();
        return patientsPurchasedMedicine;
    }

    public async Task<IEnumerable<Patient>> GetPatients2023(string nameMedicine)
    {
        var existMedicine = await _context.Medicines
                                    .Where(u => u.Name.ToLower() == nameMedicine.ToLower())
                                    .FirstOrDefaultAsync();
        if (existMedicine == null)
        {
            return null;
        }
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();

        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();

        var patientsPurchasedMedicine = (from patient in patients
                                         join sale in sales on patient.Id equals sale.PatientId
                                         join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                                         join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                                         where medicine.Name == nameMedicine
                                         select patient).Distinct();
        return patientsPurchasedMedicine;
    }

    public async Task<IEnumerable<object>> GetlessSoldMedicine()
    {
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();

        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();

        var groupMedicine = (from medicine in medicines
                             join saleMedicine in salesMedicines on medicine.Id equals saleMedicine.MedicineId
                             select medicine).GroupBy(u => u.Id);
        Dictionary<int, int> cantMedicines = new();
        foreach (var group in groupMedicine)
        {
            cantMedicines.Add(group.Key, group.Count());
        }
        int lessQuantity = cantMedicines.Values.Min();
        List<object> lessSoldMedicine = new();

        foreach (var dic in cantMedicines)
        {
            if (lessQuantity == dic.Value)
            {
                var medicine = await _context.Medicines.Where(u => u.Id == dic.Key).FirstOrDefaultAsync();
                object objecResult = new
                {
                    medicine.Id,
                    medicine.Name,
                    medicine.Stock,
                    LessQuantity = dic.Value
                };

                lessSoldMedicine.Add(objecResult);
            };
        };

        return lessSoldMedicine;
    }

    public async Task<IEnumerable<object>> GetPatientTotalSpent()
    {//2023
        DateTime init2023 = new(2023, 1, 1);
        DateTime init2024 = new(2024, 1, 1);
        var sales = await _context.Sales
                                    .Where(u => u.DateSale >= init2023 && u.DateSale < init2024).ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();

        var groupSalesMedicine = (
                        from sale in sales
                        join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                        join medicine in medicines on saleMedicine.Id equals medicine.Id
                        select saleMedicine).GroupBy(u => u.SaleId);


        Dictionary<int, double> spentPatient = new();
        foreach (var group in groupSalesMedicine)
        {
            double spentSale = 0;
            foreach (var saleMedicine in group)
            {
                spentSale += saleMedicine.Price;
            }

            int idPatient = sales.Where(u => u.Id == group.Key).FirstOrDefault().PatientId;
            Console.WriteLine(idPatient);

            if (spentPatient.ContainsKey(idPatient))
            {

                spentPatient[idPatient] += spentSale;
            }
            else
            {
                spentPatient.Add(idPatient, spentSale);
            }
        }

        foreach (var patient in patients)
        {
            if (!spentPatient.ContainsKey(patient.Id))
            {
                spentPatient.Add(patient.Id, 0);
            }
        }
        List<object> totalSpent = new();

        foreach (var dic in spentPatient)
        {
            var patient = patients.Where(u => u.Id == dic.Key).FirstOrDefault();
            object objecResult = new
            {
                patient.Id,
                patient.Name,
                TotalSpent = dic.Value
            };
            totalSpent.Add(objecResult);

        }

        return totalSpent;


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
        var quantity = employees.Select(u => new { u.Name, u.Sales.Count });

        return quantity;
    }
     public async Task<IEnumerable<object>> GetTotalMedicinesQuarter(int quarterM)
    {
        if(quarterM<= 0 || quarterM>=5)
        {
            return null;
        }
        int init  =1;
        switch(quarterM)
        {
            case 1: init = 1; break;
            case 2: init = 4; break;
            case 3: init = 7; break;
            case 4: init = 10; break;
            default:
                break;
        }
        DateTime dateStart = new(2023,init,1);
        DateTime dateEnd = dateStart.AddMonths(3);

        var sales = await _context.Sales
                            .Where(u=> u.DateSale>= dateStart && u.DateSale<= dateEnd)
                            .ToListAsync();
        var saleMedicines =await _context.SaleMedicines.ToListAsync();
        var medicines = await _context.Medicines.ToListAsync();
        
        var groupMedicines = (from sale in sales 
                             join saleMedicine in saleMedicines on sale.Id equals saleMedicine.SaleId
                             join medicine in medicines on saleMedicine.MedicineId equals medicine.Id
                             select saleMedicine).GroupBy(u=> u.MedicineId);
        
        Dictionary<int,int> cantMedicines = new();

        foreach(var group in groupMedicines)
        {
            int cantMed = 0;
            foreach(var saleMedicine in group)
            {
                cantMed += saleMedicine.SaleQuantity;
            }

            int idMedicine = saleMedicines.Where(u=>u.MedicineId == group.Key).FirstOrDefault().MedicineId;

            if(cantMedicines.ContainsKey(idMedicine)){
                
                cantMedicines[idMedicine] += cantMed;
            }else{
                cantMedicines.Add(idMedicine,cantMed);
            }
        }
        foreach(var medicine in medicines)
        {
            if(!cantMedicines.ContainsKey(medicine.Id))
            {
                cantMedicines.Add(medicine.Id,0);
            }
        }
        List<object>totalSpent = new();

        foreach(var dic in cantMedicines)
        {
            var medicine = medicines.Where(u=> u.Id == dic.Key).FirstOrDefault();
            object objecResult = new{
                medicine.Id,
                medicine.Name,
                TotalQuantity = dic.Value
            };
            totalSpent.Add(objecResult);

        }


        return totalSpent;
    }

    public async Task<IEnumerable<object>> GetPatientMoreSpent()
    {//2023
        DateTime init2023 = new(2023,1,1);
        DateTime init2024 = new(2024,1,1);
        var sales = await _context.Sales
                                    .Where(u=> u.DateSale>= init2023 && u.DateSale< init2024).ToListAsync();

        var medicines = await _context.Medicines.ToListAsync();
        var patients = await _context.Patients.ToListAsync();
        var salesMedicines = await _context.SaleMedicines.ToListAsync();
        var groupSalesMedicine = (
                        from sale in sales
                        join saleMedicine in salesMedicines on sale.Id equals saleMedicine.SaleId
                        join medicine in medicines on saleMedicine.Id equals medicine.Id
                        select saleMedicine).GroupBy(u=> u.SaleId);

        Dictionary<int,double> spentPatient = new();
        foreach(var group in groupSalesMedicine)
        {
            double spentSale = 0;
            foreach(var saleMedicine in group)
            {
                 spentSale += saleMedicine.Price;
            }

            int idPatient = sales.Where(u=>u.Id == group.Key).FirstOrDefault().PatientId;
            Console.WriteLine(idPatient);

            if(spentPatient.ContainsKey(idPatient)){
                
                spentPatient[idPatient] += spentSale;
            }else{
                spentPatient.Add(idPatient,spentSale);
            }
        }

        foreach(var patient in patients)
        {
            if(!spentPatient.ContainsKey(patient.Id))
            {
                spentPatient.Add(patient.Id,0);
            }
        }
        
        double maxSpent = spentPatient.Max(patient => patient.Value);

        List<object>totalSpent = new();

        foreach(var dic in spentPatient)
        {
            if(dic.Value == maxSpent)
            {
            var patient = patients.Where(u=> u.Id == dic.Key).FirstOrDefault();
            object objecResult = new{
                patient.Id,
                patient.Name,
                TotalSpent = dic.Value
            };
            totalSpent.Add(objecResult);
            }
            
        }

        return totalSpent;
    }

}