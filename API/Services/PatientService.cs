using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace API.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterAsync(PatientDto patientDto)
        {
            var patient = new Patient
            {
                Name = patientDto.Name,
                IdenNumber = patientDto.IdenNumber,
                PhoneNumber = patientDto.PhoneNumber,
                Address = patientDto.Address
            };

            var existingID = _unitOfWork.Patients
                                    .Find(u => u.IdenNumber.ToLower() == patientDto.IdenNumber.ToLower())
                                    .FirstOrDefault();
            
            if (existingID == null)
            {
                try
                {
                    _unitOfWork.Patients.Add(patient);
                    await _unitOfWork.SaveAsync();

                    return $"User  {patientDto.Name} has been registered successfully";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }else{
                return $"Patient with Identifaction Number {patientDto.IdenNumber} alredy register ";
            }
        
        }

        public async Task<string> UpdateAsync(PatientPutDto patientPutDto){
            
            var patient = await _unitOfWork.Patients.GetByIdAsync(patientPutDto.Id);

            patient.Name = patientPutDto.Name;
            patient.PhoneNumber = patientPutDto.PhoneNumber;
            patient.Address = patientPutDto.Address;
    
            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.SaveAsync();

            return "patient update successfully";

        }

        
    }
}