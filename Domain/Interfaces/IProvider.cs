using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;
    public interface IProvider: IGenericRepository<Provider>
<<<<<<< HEAD
{}
=======
{
        Task<Provider> GetByIdProviderAsync(int id);

}
>>>>>>> origin/main
