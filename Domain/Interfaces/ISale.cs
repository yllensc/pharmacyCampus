using System;
using System.Collections;
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
    Task<object> GetAverage();
    Task<object> GetSaleQuantityAsync();
    // Task<IEnumerable<object>> GetAverage();
    Task<object> GetTotalSalesOneMedicine(int id);
    Task<object> GetGainSales();
    Task<IEnumerable<Medicine>> GetUnsoldMedicines2023();
    Task<IEnumerable<Medicine>> GetUnsoldMedicine();
    Task<IEnumerable<Patient>> GetPatients(int id);
    Task<IEnumerable<Patient>> GetPatients2023(int id);
    Task<IEnumerable<object>> GetlessSoldMedicine();    
    Task<IEnumerable<object>> GetPatientTotalSpent();
    Task<object> GetTotalMedicinesQuarter(int quarter);

    Task<IEnumerable<object>> GetPatientMoreSpent();

    Task<IEnumerable<object>> GetBatchOfMedicines();
    Task<object> GetAllSales();




}
