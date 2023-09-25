using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface ISale : IGenericRepository<Sale>
{
    Task<string> RegisterAsync(Sale modelSale, SaleMedicine modelSaleMedicine);
    Task<string> RegisterManyMedicinesAsync(Sale modelSale , List<SaleMedicine> list);
    Task<IEnumerable<Sale>> GetAllRecipesAsync();
    Task<IEnumerable<Sale>> GetSaleMonthly(int parameter);
    // Task<IEnumerable<object>> GetAverage();
    Task<object> GetTotalSalesOneMedicine(string medicine);
    Task<object> GetGainSales();
    Task<IEnumerable<Medicine>> GetUnsoldMedicines2023();
    Task<IEnumerable<Medicine>> GetUnsoldMedicine();
    Task<IEnumerable<Patient>> GetPatients(string nameMedicine);


}
