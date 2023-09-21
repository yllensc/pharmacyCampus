using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PurchasedMedicine 
    {
        public int PurchasedId { get; set;}
        public Purchase Purchase{ get; set;}
        public int MedicineId {get; set;}
        public Medicine Medicine { get; set;}
        public int CantPurchased { get; set;}
        public int Stock { get; set;}
        public double PricePurchase { get; set;}
        public DateTime ExpirationDate { get; set; }


    }
}