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
    public class PurchaseRepository: GenericRepository<Purchase>, IPurchase
{
    private readonly PharmacyDbContext _context;

    public PurchaseRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }

    public async Task<Purchase> GetByDate(DateTime date)
    {
        return await _context.Purchases
                    .Include(p=>p.PurchasedMedicines)
                    .FirstAsync(u => u.DatePurchase == date);
    }

    public override async Task<IEnumerable<Purchase>> GetAllAsync()
    {
        return await _context.Purchases
            .Include(p=>p.PurchasedMedicines).ThenInclude(p=>p.Medicine)
            .ToListAsync();
    }

    public override async Task<Purchase> GetByIdAsync(int id)
    {
        return await _context.Purchases
            .Include(p=>p.PurchasedMedicines).ThenInclude(p=>p.Medicine)
            .FirstOrDefaultAsync(p=>p.Id == id);
    }

     public async Task<string> RegisterAsync(Purchase modelPurchase, PurchasedMedicine modelPurMedicine)
    {
        string strExpirationDate= modelPurMedicine.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        
        if (DateTime.TryParseExact(strExpirationDate, "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate))
        {
            var newPurchase = new Purchase
            {
                DatePurchase = DateTime.UtcNow,
                ProviderId = modelPurchase.ProviderId
            };

            try{
                _context.Purchases.Add(newPurchase);
                await _context.SaveChangesAsync();

                var purchaseCreated = await _context.Purchases
                                        .Where(u=> u.DatePurchase == newPurchase.DatePurchase)
                                        .FirstOrDefaultAsync();

                var newPurchaseMedicine = new PurchasedMedicine
                {
                    PurchasedId = purchaseCreated.Id,
                    MedicineId = modelPurMedicine.MedicineId,
                    CantPurchased = modelPurMedicine.CantPurchased,
                    PricePurchase = modelPurMedicine.PricePurchase,
                    Stock = modelPurMedicine.CantPurchased,
                    ExpirationDate = parseDate
                };
                try{
                
                _context.PurchasedMedicines.Add(newPurchaseMedicine);
                await _context.SaveChangesAsync();

                var medicine = await _context.Medicines
                                    .Where(u=>u.Id == modelPurMedicine.MedicineId)
                                    .FirstOrDefaultAsync();

                medicine.Stock += modelPurMedicine.CantPurchased;
                 _context.Medicines.Update(medicine);
                await _context.SaveChangesAsync();

                }catch(Exception ex){
                    _context.Purchases.Remove(newPurchase);
                    _context.PurchasedMedicines.Remove(newPurchaseMedicine);

                    return ex.Message;
                } 
            }catch(Exception ex)
            {
                _context.Purchases.Remove(newPurchase);

                return ex.Message;
            }
            
            return "Purchase made successfully!!";
        }else
        {
            return "Expiration Date hasn't correct format";
        }
       
    }

     public async Task<string> RegisterManyMedicinesAsync(Purchase modelPurchase,  List<PurchasedMedicine> list)
    {

        foreach(var purshaseMedicine in list)
        {
            string strExpirationDate= purshaseMedicine.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");

            if(!DateTime.TryParseExact(strExpirationDate, "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate)){
                
                return "Expiration date with incorrect format. Please, fix it";
            }
            purshaseMedicine.ExpirationDate = parseDate;

        }
        var newPurchase = new Purchase
        {
            DatePurchase = DateTime.UtcNow,
            ProviderId = modelPurchase.ProviderId
        };

        try{
            _context.Purchases.Add(newPurchase);
            await _context.SaveChangesAsync();

            var purchaseCreated = await _context.Purchases
                                        .Where(u=> u.DatePurchase == newPurchase.DatePurchase)
                                        .FirstOrDefaultAsync();
            List<PurchasedMedicine> newPmedicines = new(); 
            foreach(var purshaseMedicine in list)
            {
                newPmedicines.Add(new PurchasedMedicine
                {
                    PurchasedId = purchaseCreated.Id,
                    MedicineId = purshaseMedicine.MedicineId,
                    CantPurchased = purshaseMedicine.CantPurchased,
                    PricePurchase = purshaseMedicine.PricePurchase,
                    Stock = purshaseMedicine.CantPurchased,
                    ExpirationDate =purshaseMedicine.ExpirationDate
                });

                var medicine = await _context.Medicines
                                    .Where(u=>u.Id == purshaseMedicine.MedicineId)
                                    .FirstOrDefaultAsync();

                medicine.Stock += purshaseMedicine.CantPurchased;
                _context.Medicines.Update(medicine);
                await _context.SaveChangesAsync();
            } 
            try{
            _context.PurchasedMedicines.AddRange(newPmedicines);
            await _context.SaveChangesAsync();

            }catch(Exception ex){
                _context.Purchases.Remove(newPurchase);
                _context.PurchasedMedicines.RemoveRange(newPmedicines);
                return ex.Message;
            } 

        }catch(Exception ex)
        {
            _context.Purchases.Remove(newPurchase);
            return ex.Message;
        }
        
        return "Purchase made successfully!!";
    }

    public async Task<IEnumerable<Provider>> GetProvidersWithoutPurchases()
    {
        DateTime date = DateTime.UtcNow.AddYears(-1);
        var purchases = await _context.Purchases
                            .Where(u=> u.DatePurchase > date)
                            .ToListAsync();
        
        var providers = _context.Providers;

        List<Provider> result = new();
        foreach(var provider in providers)
        {
            var existProvider = purchases
                                    .Where(u=> u.ProviderId == provider.Id)
                                    .FirstOrDefault();
            if(existProvider == null)
            {
                result.Add(provider);
            }
        }

       return result;
    }
    public async Task<IEnumerable<Medicine>> GetMedicinesPurchasedByProvider(int idprovider)
    {
        List<Medicine> medicinesProvider = new();
        var providerM = await _context.Providers
                                .Where(u=>u.Id == idprovider)
                                .FirstOrDefaultAsync();
        if(providerM == null)
        {
            return null;
        }
        var existPurchase = await _context.Purchases
                                            .Where(u=> u.ProviderId == idprovider)
                                          .ToListAsync();
        
        List<int> IdsMedicine = new();
        if(existPurchase != null)
        {
            foreach(var purchase in existPurchase)
            {
                var purMedicines = await _context.PurchasedMedicines
                                                .Where(u=> u.PurchasedId == purchase.Id)
                                                .ToListAsync();
                foreach(var pMed in purMedicines)
                {
                    int idMedicine = pMed.MedicineId;
                    if(!IdsMedicine.Contains(idMedicine))
                    {
                        IdsMedicine.Add(idMedicine);
                    }
                }   
            }

            foreach(var id in IdsMedicine)
            {
                var medicine =await _context.Medicines.Where(u=>u.Id == id).FirstOrDefaultAsync();
                medicinesProvider.Add(medicine);
            }
        }
        
        return medicinesProvider;
    }
}

