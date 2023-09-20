using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

    public class Sale : BaseEntity
    {
        public DateTime DateSale { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public bool Prescription { get; set; } = false;
        public ICollection<SaleMedicine> SaleMedicines{ get; set; }

    }
