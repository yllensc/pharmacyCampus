using API.Dtos;
namespace API.Services;
public interface IUserService
{
    Task<string> RegisterAsync(RegisterDto model);
    Task<string> RegisterAdmiAsync(RegisterAdmiDto model);
    Task<string> RegisterEmployeeAsync(RegisterEmployeeDto model);
    Task<string> RegisterPatientAsync (RegisterPatientDto model);
    Task<DataUserDto> GetTokenAsync(LoginDto model);
    Task<string> AddRoleAsync(AddRoleDto model);
    Task<DataUserDto> RefreshTokenAsync(string refreshToken);
}
