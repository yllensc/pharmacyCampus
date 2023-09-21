using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities;

    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public string Address   { get; set; }
        public string PhoneNumber { get; set; }
        public string IdenNumber { get; set; }
        public ICollection<Sale> Sales { get; set;}

    }
