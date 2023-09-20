using API.Dtos;
namespace API.Services;
public interface IUserService
{
    Task<string> RegisterAsync(RegisterDto model);
<<<<<<< HEAD
    Task<string> RegisterAdmiAsync(RegisterAdmiDto model);
    Task<string> RegisterEmployeeAsync(RegisterEmployeeDto model);
<<<<<<< HEAD
<<<<<<< HEAD
    Task<string> RegisterPatientAsync (RegisterPatientDto model);
=======
   // Task<string> RegisterPatientAsync (RegisterPatientDto model);
>>>>>>> a05ced1 (Modification  relations in User to employee and patient)
=======
    Task<string> RegisterPatientAsync (RegisterPatientDto model);
>>>>>>> 71f46c7 (feat: :sparkles: Register Patient!)
<<<<<<< HEAD
=======
>>>>>>> origin/main
=======
>>>>>>> 1f490af (feat: :sparkles: Register Patient!)
    Task<DataUserDto> GetTokenAsync(LoginDto model);
    Task<string> AddRoleAsync(AddRoleDto model);
    Task<DataUserDto> RefreshTokenAsync(string refreshToken);
}
