using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;
    public class Purchase: BaseEntity
    {
        public DateTime DatePurchase { get; set; }
        public int ProviderId { get; set;}
        public Provider Provider{ get; set; }
        public ICollection<PurchasedMedicine> PurchasedMedicines{ get; set; }
    }
