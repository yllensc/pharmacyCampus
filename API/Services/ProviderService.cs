using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace API.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProviderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> RegisterAsync(ProviderDto providerDto)
        {
            var provider = new Provider
            {
                Name = providerDto.Name,
                IdenNumber = providerDto.IdenNumber,
                Email = providerDto.Email,
                Address = providerDto.Address
            };

            var existingID = _unitOfWork.Providers
                                    .Find(u => u.IdenNumber.ToLower() == providerDto.IdenNumber.ToLower())
                                    .FirstOrDefault();
            
            if (existingID == null)
            {
                try
                {
                    _unitOfWork.Providers.Add(provider);
                    await _unitOfWork.SaveAsync();

                    return $"User  {providerDto.Name} has been registered successfully";
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return $"Error: {message}";
                }
            }else{
                return $"Provider with Identifaction Number {providerDto.IdenNumber} alredy register ";
            }
        
        }

          public async Task<string> UpdateAsync(ProviderPutDto providerPutDto){
            
            var provider = await _unitOfWork.Providers.GetByIdAsync(providerPutDto.Id);

            provider.Name = providerPutDto.Name;
            provider.Email = providerPutDto.Email;
            provider.Address = providerPutDto.Address;
    
            _unitOfWork.Providers.Update(provider);
            await _unitOfWork.SaveAsync();

            return "Provider update successfully";

        }
    }
}