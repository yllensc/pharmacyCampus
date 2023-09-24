using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
    public class ProviderRepository: GenericRepository<Provider>, IProvider
{
    private readonly PharmacyDbContext _context;

    public ProviderRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }
     public override async Task<IEnumerable<Provider>> GetAllAsync()
    {
        return await _context.Providers
            .Include(p=>p.Purchases).ThenInclude(p=>p.PurchasedMedicines).ThenInclude(p=>p.Medicine)
            .ToListAsync();
    }

    public override async Task<Provider> GetByIdAsync(int id)
    {
        return await _context.Providers
            .Include(p=>p.Purchases).ThenInclude(p=>p.PurchasedMedicines)
            .FirstOrDefaultAsync(p=>p.Id == id);
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
        }else{
            return $"Provider with Identifaction Number {model.IdenNumber} alredy register ";
        }
    
    }

    public async Task<string> UpdateAsync(Provider model){
        
        var provider = await _context.Providers.Where(u=> u.Id == model.Id).FirstOrDefaultAsync();

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
            .Include(p=>p.Medicines)
            .ToListAsync();
    }

    public async Task<IEnumerable<double>> GetGainsByProviders()
    {
        List<double> gainsProvider = new();
        var providers = _context.Providers;
    
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
                
            }else
            {
                gainsProvider.Add(0);
            }
        }

        return gainsProvider;
    } 
}