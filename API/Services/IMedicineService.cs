using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;

namespace API.Services
{
    public interface IMedicineService
    {
        Task<string> RegisterAsync(MedicineDto model);
        Task<string> UpdateAsync(MedicinePutDto model);

    }
}