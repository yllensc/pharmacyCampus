using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MedicineDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Stock { get; set; }
        public int ProviderId { get; set; } 
        public ProviderDto Provider { get; set; }
    }

    public class MedicinePDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Stock { get; set; }
    }

    public class MedicineOnlyDto{
        [Required]
        public string Name { get; set; }
    }

     public class MedicineCantDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int LessQuantity { get; set; }

    }
}