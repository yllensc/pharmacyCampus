using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;
public interface IPurchase: IGenericRepository<Purchase>
{
    Task<string> RegisterAsync(Purchase model,  PurchasedMedicine modelPurMedicine);
    Task<string> RegisterManyMedicinesAsync(Purchase modelPurchase , List<PurchasedMedicine> list);
    
    //Ennpoints
    Task<IEnumerable<Medicine>>GetMedicinesPurchasedByProvider(int idprovider);
    Task<IEnumerable<Provider>> GetProvidersWithoutPurchases();

}