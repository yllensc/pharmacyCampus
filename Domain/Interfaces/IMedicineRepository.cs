using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IMedicineRepository : IGenericRepository<Medicine>
{
    Task<string> RegisterAsync(Medicine model);
    Task<string> UpdateAsync(Medicine model);
    Task<IEnumerable<Medicine>> GetUnder50();
    Task<IEnumerable<Medicine>> GetExpireUnder2024();
    Task<IEnumerable<Medicine>> GetExpireIn2024();
    Task<IEnumerable<Medicine>> GetMoreExpensive();
    Task<IEnumerable<Medicine>> GetRangePriceStockPredeterminated();
    Task<int> CalculateTotalPurchaseQuantity(Provider provider);  
    Task<int> CalculateTotalStockQuantity(Provider provider);
    
    
}
