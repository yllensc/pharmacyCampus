using System;
using System.Collections.Generic;
using System.Globalization;
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
        string strExpirationDate= purchasePostDto.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        
        if (DateTime.TryParseExact(strExpirationDate, "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate))
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
                    PricePurchase = purchasePostDto.PricePurchase,
                    Stock = purchasePostDto.CantPurchased,
                    ExpirationDate = parseDate
                };
                try{
                
                _unitOfWork.PurchasedMedicines.Add(newPurchaseMedicine);
                await _unitOfWork.SaveAsync();

                //Traer la info de medicina para sumarle el stock

                }catch(Exception ex){
                    return ex.Message;
                } 
            }catch(Exception ex)
            {
                return ex.Message;
            }
            
            return "Purchase made successfully!!";
        }else
        {
            return "Expiration Date hasn't correct format";
        }
       
    }

    public async Task<string> RegisterManyMedicinesAsync(PurchaseManyPostDto purchaseManyPostDto)
    {

        foreach(var purshaseMedicine in purchaseManyPostDto.MedicinesList)
        {
            string strExpirationDate= purshaseMedicine.ExpirationDate.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");

            if(!DateTime.TryParseExact(strExpirationDate, "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate)){
                
                return "Expiration date with incorrect format. Please, fix it";
            }

        }
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
                DateTime.TryParseExact(purshaseMedicine.ExpirationDate.ToString(), "yyyy-MM-ddTHH:mm:ss.ffffffZ", null, DateTimeStyles.None, out DateTime parseDate);
                newPmedicines.Add(new PurchasedMedicine
                {
                    PurchasedId = purchaseCreated.Id,
                    MedicineId = purshaseMedicine.MedicineId,
                    CantPurchased = purshaseMedicine.CantPurchased,
                    PricePurchase = purshaseMedicine.PricePurchase,
                    Stock = purshaseMedicine.CantPurchased,
                    ExpirationDate =parseDate
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
