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
<<<<<<< HEAD
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
=======
    public async Task<string> RegisterAsync(PurchaseMedicineDto purchaseMedicineDto)
    {
        var newPurshase = new Purchase
        {
            DatePurchase = purchaseMedicineDto.DatePurchase,
            ProviderId = purchaseMedicineDto.ProviderId
        };

        _unitOfWork.Purchases.Add(newPurshase);
        await _unitOfWork.SaveAsync();

        var purchaseCreated = await _unitOfWork.Purchases.GetByDate(purchaseMedicineDto.DatePurchase);
>>>>>>> eca8963 (feat: :sparkles: Add Purchase, wuuu)

        var newPurchaseMedicine = new PurchasedMedicine
        {
            PurchasedId = purchaseCreated.Id,
<<<<<<< HEAD
            MedicineId = purchasePostDto.MedicineId,
            CantPurchased = purchasePostDto.CantPurchased,
            PricePurchase = purchasePostDto.PricePurchase
=======
            MedicineId = purchaseMedicineDto.MedicineId,
            CantPurchased = purchaseMedicineDto.CantPurchased,
            PricePurchase = purchaseMedicineDto.PricePurchase
>>>>>>> eca8963 (feat: :sparkles: Add Purchase, wuuu)
        };

        _unitOfWork.PurchasedMedicines.Add(newPurchaseMedicine);
        await _unitOfWork.SaveAsync();

        return "Purchase made successfully!!";
    }

}
