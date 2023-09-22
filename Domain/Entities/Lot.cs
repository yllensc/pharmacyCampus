using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lot : BaseEntity
    {
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime ExpireDate { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}