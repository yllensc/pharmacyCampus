using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User> 
{ 
    Task<User> GetByUserNameAsync(string username);
    Task<User> GetByRefreshTokenAsync(string username);

}
