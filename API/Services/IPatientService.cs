using API.Dtos;
using Persistence.Data.Configuration;

namespace API.Services;

public interface IPatientService
{
    Task<string> RegisterAsync(PatientDto model);
    Task<string> UpdateAsync(PatientPutDto model);

}
