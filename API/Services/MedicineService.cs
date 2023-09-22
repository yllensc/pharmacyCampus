using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Interfaces;
using Domain.Entities;
using API.Dtos;
using Domain.Interfaces;
using Domain.Entities;

namespace API.Services
{
    public class MedicineService : IMedicineService
    {
private readonly IUnitOfWork _unitOfWork;
    public MedicineService(IUnitOfWork unitOfWork)
    public class MedicineService : IMedicineService
    {
private readonly IUnitOfWork _unitOfWork;
    public MedicineService(IUnitOfWork unitOfWork)
    {
        
        }

        public Task<string> UpdateAsync(MedicinePutDto model)
        {
            throw new NotImplementedException();
        }
        }

        public Task<string> UpdateAsync(MedicinePutDto model)
        {
            throw new NotImplementedException();
        }
    }
}