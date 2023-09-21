using API.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace API.Services;

public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;
    public SaleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> RegisterAsync(SaleDto saleDto)
    {
        var sale = new Sale
        {
            DateSale = saleDto.DateSale,
            PatientId = saleDto.PatientId,
            EmployeeId = saleDto.EmployeeId,
            Prescription = saleDto.Prescription
        };
 
        var existingID = _unitOfWork.Sales
                                .Find(u => u.Id == saleDto.Id)
                                .FirstOrDefault();
        
        if (existingID == null)
        {
            try
            {
                _unitOfWork.Sales.Add(sale);
                await _unitOfWork.SaveAsync();
                return $"User  {saleDto.Id} has been registered successfully";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }else{
            return $"Sale with Identifaction Number {saleDto.Id} alredy register ";
        }
    }
        
}