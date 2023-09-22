using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;

namespace API.Services
{
    public interface IEmployeeService
    {
        Task<string> UpdateAsync(EmployeeDto model);
    }
}