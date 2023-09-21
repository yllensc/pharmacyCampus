using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class PurchaseMedicineDto
{
<<<<<<< HEAD
    public string MedicineName {get; set;}
    public int CantPurchased { get; set;}
=======
    [Required]
    public int ProviderId { get; set;}
    public DateTime DatePurchase { get; set; } = DateTime.UtcNow;

    [Required]
    public int MedicineId {get; set;}
    [Required]
    public int CantPurchased { get; set;}
    [Required]
>>>>>>> eca8963 (feat: :sparkles: Add Purchase, wuuu)
    public double PricePurchase { get; set;}

}
