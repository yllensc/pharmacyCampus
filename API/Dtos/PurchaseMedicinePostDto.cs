using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class PurchaseMedicinePostDto
{
    [Required]
    public int MedicineId {get; set;}
    [Required]
    public int CantPurchased { get; set;}
    [Required]
    public double PricePurchase { get; set;}  
    [Required]
     public int Stock { get; set;}
    [Required]
    public DateTime ExpirationDate { get; set; }

}
