using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IPatient : IGenericRepository<Patient>
{
    Task<string> RegisterAsync(Patient model);
    Task<string> UpdateAsync(Patient model);
    Task<IEnumerable<Patient>> GetPatientWithNoSalesInxYear(int year);
}
