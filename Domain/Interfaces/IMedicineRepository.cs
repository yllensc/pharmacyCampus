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
    Task<IEnumerable<Medicine>> GetUnderCant(int cant);
    Task<IEnumerable<Medicine>> GetExpireUnderxYear(int year);
    Task<IEnumerable<Medicine>> GetExpireInxYear(int year);
    Task<IEnumerable<Medicine>> GetMoreExpensive();
    Task<IEnumerable<Medicine>> GetRangePriceStockPredeterminated(double price, int stock);
    Task<int> CalculateTotalPurchaseQuantity(Provider provider);  
    Task<int> CalculateTotalStockQuantity(Provider provider);
    Task<Dictionary<string, List<object>>> GetMedicineSoldonYear(int year);
    
    
}
