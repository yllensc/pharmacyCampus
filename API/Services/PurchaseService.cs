using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using Domain.Entities;
using Domain.Interfaces;

namespace API.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IUnitOfWork _unitOfWork;
    public PurchaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<string> RegisterAsync(PurchasePostDto purchasePostDto)
    {
        var newPurchase = new Purchase
        {
            DatePurchase = DateTime.UtcNow,
            ProviderId = purchasePostDto.ProviderId
        };

        _unitOfWork.Purchases.Add(newPurchase);
        await _unitOfWork.SaveAsync();

        var purchaseCreated = await _unitOfWork.Purchases.GetByDate(newPurchase.DatePurchase);

        var newPurchaseMedicine = new PurchasedMedicine
        {
            PurchasedId = purchaseCreated.Id,
            MedicineId = purchasePostDto.MedicineId,
            CantPurchased = purchasePostDto.CantPurchased,
            PricePurchase = purchasePostDto.PricePurchase
        };

        _unitOfWork.PurchasedMedicines.Add(newPurchaseMedicine);
        await _unitOfWork.SaveAsync();

        return "Purchase made successfully!!";
    }

}
