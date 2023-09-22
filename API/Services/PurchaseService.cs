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

        try{
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
            try{
            
            _unitOfWork.PurchasedMedicines.Add(newPurchaseMedicine);
            await _unitOfWork.SaveAsync();

            }catch(Exception ex){
                return ex.Message;
            } 
        }catch(Exception ex)
        {
            return ex.Message;
        }
        
        return "Purchase made successfully!!";
    }

    public async Task<string> RegisterManyMedicinesAsync(PurchaseManyPostDto purchaseManyPostDto)
    {
        var newPurchase = new Purchase
        {
            DatePurchase = DateTime.UtcNow,
            ProviderId = purchaseManyPostDto.ProviderId
        };

        try{
            _unitOfWork.Purchases.Add(newPurchase);
            await _unitOfWork.SaveAsync();

            var purchaseCreated = await _unitOfWork.Purchases.GetByDate(newPurchase.DatePurchase);

            List<PurchasedMedicine> newPmedicines = new(); 
            foreach(var purshaseMedicine in purchaseManyPostDto.MedicinesList)
            {
                newPmedicines.Add(new PurchasedMedicine
                {
                    PurchasedId = purchaseCreated.Id,
                    MedicineId = purshaseMedicine.MedicineId,
                    CantPurchased = purshaseMedicine.CantPurchased,
                    PricePurchase = purshaseMedicine.PricePurchase
                });
            } 
                try{
                _unitOfWork.PurchasedMedicines.AddRange(newPmedicines);
                await _unitOfWork.SaveAsync();

                }catch(Exception ex){
                    return ex.Message;
                } 

        }catch(Exception ex)
        {
            return ex.Message;
        }
        
        return "Purchase made successfully!!";
    }

    
}
