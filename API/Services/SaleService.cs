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

    public async Task<string> RegisterAsync (SaleDto saleDto)
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
                var saleMedicine = new SaleMedicine
                {
                    SaleId = sale.Id,
                    MedicineId = saleDto.MedicineId,
                    SaleQuantity = saleDto.QuantitySale,
                    Price = saleDto.Price
                };

                var existingMedicine = _unitOfWork.Medicines
                                .Find(u => u.Id == saleDto.Id)
                                .FirstOrDefault();

                if (existingMedicine != null)
                {
                    try
                    {
                        if (existingMedicine.Stock >= saleDto.QuantitySale)
                        {
                            _unitOfWork.Sales.Add(sale);
                            _unitOfWork.SaleMedicines.Add(saleMedicine);
                            existingMedicine.Stock = existingMedicine.Stock - saleDto.QuantitySale;  
                            _unitOfWork.Medicines.Update(existingMedicine);
                            await _unitOfWork.SaveAsync();
                            return $"User {saleDto.Id} has been registered successfully";
                        }else{
                            return $"Sale with Identifaction Number {saleDto.Id} alredy register ";
                        }
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