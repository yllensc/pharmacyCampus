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
                return ex.Message;
            } 
        }catch(Exception ex)
        {
            return ex.Message;
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
}
