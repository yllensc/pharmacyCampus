using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;
    public interface IProvider: IGenericRepository<Provider>
{
        Task<Provider> GetByIdProviderAsync(int id);

}