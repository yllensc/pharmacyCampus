using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

public class PMedicineGetDto
{

    public string MedicineName { get; set; }
    public int CantPurchased { get; set;}
    public double PricePurchase { get; set;}
}
