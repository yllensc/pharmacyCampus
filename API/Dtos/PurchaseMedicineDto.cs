using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class PurchaseMedicineDto
{
    public string MedicineName {get; set;}
    public int CantPurchased { get; set;}
    public double PricePurchase { get; set;}
    public int Stock { get; set;} 
    public DateTime ExpirationDate { get; set; }

}
