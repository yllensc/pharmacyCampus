using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User> 
{ 
    Task<int> GetIDUserAsync(string username);
    Task<User> GetByUserNameAsync(string username);
    Task<User> GetByRefreshTokenAsync(string username);
    Task<IEnumerable<User>> GetAllRolesAsync();

}
