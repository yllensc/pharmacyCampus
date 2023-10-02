using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProviderNameOnlyDto{
        public string Name { get; set; }

    }
    public class ProviderPutDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        [Required]
        public string Email { get; set;}
        public string Address { get; set;}
    }
    public class ProviderDto : ProviderPutDto
    {
        [Required]
        public string IdenNumber { get; set;}
    }
    public class ProviderWithMedicineUnder50: ProviderPutDto
    {
       public IEnumerable<MedicineWithStockDto> MedicinesList{ get; set;} 
    }

    public class ProviderWithListMedicinesDto : ProviderDto
    {
        public IEnumerable<MedicineBaseDto> MedicinesList{ get; set;}
    }

    public class ProviderWithTotalQuantityDto
    {
    public string Name { get; set; }
    public int Id { get; set;}
    public ICollection<MedicineWithQuantityDto> MedicinesList { get; set; }
    public int TotalPurchaseCant { get; set; }
    }
    public class ProviderWithTotalQuantityStockDto
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<MedicineWithStockDto> MedicinesList { get; set; }
    public int TotalStockCant { get; set; }
    }
}