using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;
public interface IProvider: IGenericRepository<Provider>
{
    Task<string> RegisterAsync(Provider model);
    Task<string> UpdateAsync(Provider model);
}
