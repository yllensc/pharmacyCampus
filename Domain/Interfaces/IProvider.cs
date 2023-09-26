using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;
public interface IProvider: IGenericRepository<Provider>
{
    Task<string> RegisterAsync(Provider model);
    Task<string> UpdateAsync(Provider model);
    Task<IEnumerable<Provider>> GetProvidersWithMedicines();
    Task<IEnumerable<Provider>> GetCantMedicineByProvider();
    Task<IEnumerable<Provider>> GetCantPurchasedMedicineByProvider();
    Task<IEnumerable<Provider>> GetProvidersWithMedicinesUnderx(int cant);
    
   // Task<Dictionary<string, double>> GetGainsByProviders();

    Task<IEnumerable<object>> GetProviderWithMoreMedicines();
    Task<object> GetTotalProviders2023();

    Task<IEnumerable<Provider>> GetProvidersWithDiferentMedicines();
    Task<IEnumerable<object>> GetGainByProvider();

}
