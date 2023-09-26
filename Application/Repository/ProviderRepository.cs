using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Globalization;


namespace Application.Repository;
public class ProviderRepository : GenericRepository<Provider>, IProvider
{
    private readonly PharmacyDbContext _context;

    public ProviderRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }
    public override async Task<IEnumerable<Provider>> GetAllAsync()
    {
        return await _context.Providers
            .Include(p => p.Purchases).ThenInclude(p => p.PurchasedMedicines).ThenInclude(p => p.Medicine)
            .ToListAsync();
    }

    public override async Task<Provider> GetByIdAsync(int id)
    {
        return await _context.Providers
            .Include(p => p.Purchases).ThenInclude(p => p.PurchasedMedicines)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<string> RegisterAsync(Provider model)
    {
        var provider = new Provider
        {
            Name = model.Name,
            IdenNumber = model.IdenNumber,
            Email = model.Email,
            Address = model.Address
        };

        var existingID = _context.Providers
                                .Where(u => u.IdenNumber.ToLower() == model.IdenNumber.ToLower())
                                .FirstOrDefault();

        if (existingID == null)
        {
            try
            {
                _context.Providers.Add(provider);
                await _context.SaveChangesAsync();

                return $"User  {model.Name} has been registered successfully";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"Provider with Identifaction Number {model.IdenNumber} alredy register ";
        }

    }

    public async Task<string> UpdateAsync(Provider model)
    {

        var provider = await _context.Providers.Where(u => u.Id == model.Id).FirstOrDefaultAsync();

        provider.Name = model.Name;
        provider.Email = model.Email;
        provider.Address = model.Address;

        _context.Providers.Update(provider);
        await _context.SaveChangesAsync();

        return "Provider update successfully";

    }
    public async Task<IEnumerable<Provider>> GetProvidersWithMedicines()
    {
        return await _context.Providers
            .Include(p => p.Medicines)
            .ToListAsync();
    }

    public async Task<IEnumerable<Provider>> GetCantPurchasedMedicineByProvider()
    {
        var providers = await _context.Providers
         .Include(p => p.Medicines)
         .ThenInclude(m => m.PurchasedMedicines)
         .ToListAsync();

        return providers;
    }
    public async Task<IEnumerable<Provider>> GetProvidersWithMedicinesUnderx(int cant)
    {
        var providersWithMedicinesUnder50 = await _context.Providers
            .Where(p => p.Medicines.Any(m => m.Stock < cant))
            .Select(p => new Provider
            {
                Id = p.Id,
                Name = p.Name,
                Email = p.Email,
                Address = p.Address,
                Medicines = p.Medicines.Where(m => m.Stock < 50).ToList()
            })
            .ToListAsync();

        return providersWithMedicinesUnder50;
    }

    public async Task<IEnumerable<Provider>> GetCantMedicineByProvider()
    {
        return await _context.Providers
        .Include(p => p.Medicines)
        .ToListAsync();
    }
    /*public async Task<Dictionary<string, double> > GetGainsByProviders()
    {
        List<double> gainsProvider = new();
        Dictionary<string, double> gainsProviders =new();
        var providers = await _context.Providers.ToListAsync();
    
        foreach(var p in providers)
        {
            var existPurchase = await _context.Purchases
                                            .Where(u=> u.ProviderId == p.Id)
                                            .ToListAsync();
            
            if(existPurchase !=  null)
            {

                double gains = 0;
                foreach(var purchase in existPurchase)
                {
                    var purMedicines = await _context.PurchasedMedicines
                                                    .Where(u=> u.PurchasedId == purchase.Id)
                                                    .ToListAsync();
                    foreach(var pMed in purMedicines)
                    {
                        gains += pMed.PricePurchase;
                    }
                }
                gainsProvider.Add(gains);
                gainsProviders.Add(p.Name,gains);
                
            }else
            {
                gainsProvider.Add(0);
                gainsProviders.Add(p.Name,0);

            }
        }

        return gainsProviders;
    } */

    public async Task<IEnumerable<object>> GetProviderWithMoreMedicines()
    {
        var providers = await _context.Providers.ToListAsync();
        Dictionary<int,int> cantMedicines = new();
        DateTime init2023 = new(2023,1,1);
        DateTime init2024 = new(2024,1,1);

        foreach(var p in providers)
        {
            var existPurchase = await _context.Purchases
                                            .Where(u=> u.ProviderId == p.Id && u.DatePurchase>= init2023 && u.DatePurchase< init2024 )
                                           .ToListAsync();
            List<int> IdsMedicine = new();
            int cant = 0;

            if(existPurchase.Any())
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
                            cant +=1;
                        }
                    }   
                }
                cantMedicines.Add(p.Id, cant);
            }
        }
        int moreQuantity = cantMedicines.Values.Max();
        List<object>providerWithMoreMed = new();
        foreach(var dic in cantMedicines)
        {
            if(moreQuantity == dic.Value){
                var provider = await _context.Providers.Where(u=>u.Id == dic.Key).FirstOrDefaultAsync();
                object objecResult = new{
                    provider.Id,
                    provider.Name,
                    MoreQuantity = dic.Value
                };

                providerWithMoreMed.Add(objecResult);
            };
        };

       return providerWithMoreMed;

    }

    public async Task<object> GetTotalProviders2023()
    {
        var providers = await _context.Providers.ToListAsync();
        DateTime init2023 = new(2023,1,1);
        DateTime init2024 = new(2024,1,1);
        int totalProviders = 0;
        foreach(var p in providers)
        {
            var existPurchase = await _context.Purchases
                                            .Where(u=> u.ProviderId == p.Id && u.DatePurchase>= init2023 && u.DatePurchase< init2024 )
                                           .ToListAsync();
            if(existPurchase.Any() )
            {   
                 totalProviders += 1;   
            }
        }

        object total = new{
            TotalProviders = totalProviders
        };
        return total;
    }

    public async Task<IEnumerable<Provider>> GetProvidersWithDiferentMedicines()
    {
        var providers = await _context.Providers.ToListAsync();
        Dictionary<int,int> cantMedicines = new();
        DateTime init2023 = new(2023,1,1);
        DateTime init2024 = new(2024,1,1);

        foreach(var p in providers)
        {
            var existPurchase = await _context.Purchases
                                            .Where(u=> u.ProviderId == p.Id && u.DatePurchase>= init2023 && u.DatePurchase< init2024 )
                                           .ToListAsync();
            List<int> IdsMedicine = new();
            if(existPurchase !=  null)
            {   
                int cant = 0;
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
                            cant +=1;
                        }
                    }   
                }
                cantMedicines.Add(p.Id, cant);
            }
        }
        List<Provider> providersM = new();
        foreach(var dic in cantMedicines)
        {
            if(dic.Value>=4){
                var provider = await _context.Providers.Where(u=>u.Id == dic.Key).FirstOrDefaultAsync();
                
                providersM.Add(provider);
            };
        };

       return providersM;
    }

    public async Task<IEnumerable<object>> GetGainByProvider()
    {
        List<object> gainsProviders =new();
        var providers = await _context.Providers.ToListAsync();
        DateTime init2023 = new(2023,1,1);
        DateTime init2024 = new(2024,1,1);
    
        foreach(var p in providers)
        {
            var existPurchase = await _context.Purchases
                                            .Where(u=> u.ProviderId == p.Id && u.DatePurchase>= init2023 && u.DatePurchase< init2024 )
                                           .ToListAsync();
            

            
            if(existPurchase.Any())
            {

                double gains = 0;
                foreach(var purchase in existPurchase)
                {
                    var purMedicines = await _context.PurchasedMedicines
                                                    .Where(u=> u.PurchasedId == purchase.Id)
                                                    .ToListAsync();
                    foreach(var pMed in purMedicines)
                    {
                        gains += pMed.PricePurchase;
                    }
                }

                object newObject = new{
                    p.Id,
                    p.Name,
                    TotalGain2023 = gains
                };
                gainsProviders.Add(newObject);

                
            }else
            {
                object newObject = new{
                    p.Id,
                    p.Name,
                    TotalGain2023 = 0
                };
                gainsProviders.Add(newObject);

            }

        }

        return gainsProviders;
    }

}