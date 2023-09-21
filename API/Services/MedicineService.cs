using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Interfaces;
using Domain.Entities;

namespace API.Services
{
    public class MedicineService : IMedicineService
    {
private readonly IUnitOfWork _unitOfWork;
    public MedicineService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

        public async Task<string> RegisterAsync(MedicineDto model)
        {
           var medicine = new Medicine
        {
            Name = model.Name,
<<<<<<< HEAD
=======
            Price = model.Price,
            Stock = model.Stock,
            ExpirationDate = model.ExpirationDate,
>>>>>>> 81e6bcc (Employee CRUD check y avance de Medicine jaja)
            ProviderId = model.ProviderId
        };

         var existMedicine = _unitOfWork.Medicines
                                    .Find(u => u.Name.ToLower() == model.Name.ToLower() && u.ProviderId == model.ProviderId)
                                    .FirstOrDefault();
            
            if (existMedicine == null)
            {
                try
                {
                    _unitOfWork.Medicines.Add(medicine);
                    await _unitOfWork.SaveAsync();

                    return $"Medicine  {model.Name} has been registered successfully";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }else{
                return $"Medicine {model.Name} alredy register with this provider";
            }
        
        
        }

        public Task<string> UpdateAsync(MedicinePutDto model)
        {
            throw new NotImplementedException();
        }
    }
}