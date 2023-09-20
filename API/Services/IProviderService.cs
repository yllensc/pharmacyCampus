using API.Dtos;

namespace API.Services;

public interface IProviderService
{
    Task<string> RegisterAsync(ProviderDto model);

}
