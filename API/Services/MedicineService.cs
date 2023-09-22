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
            var existingID = _unitOfWork.Providers
                        .Find(u => u.Id == model.ProviderId)
                        .FirstOrDefault();

            if (existingID != null) {
                var medicine = new Medicine
            {
                Name = model.Name,
                ProviderId = model.ProviderId,
                Stock = model.Stock,
                Price = model.Price
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
            }
            else
            {
                return $"Medicine {model.Name} alredy register with this provider";
            }
             }
             else{
                return "El proveedor no existe en nuestro sistema, sorry";
             }
        }

        public async Task<string> UpdateAsync(MedicinePutDto model)
        {
            var medicine = await _unitOfWork.Medicines.GetByIdAsync(model.Id);
            if(medicine != null){
                medicine.Stock = model.Stock;
                medicine.Price = model.Price;
                _unitOfWork.Medicines.Update(medicine);
                await _unitOfWork.SaveAsync();
                return "medicine update successfully";

            }
            else{
                return "El registro que quieres actualizar, no existe, sorry";
            }

    
            
        }
    }
}