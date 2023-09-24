using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProviderPutDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set;}
        [Required]
        public string Address { get; set;}
    }
    public class ProviderDto : ProviderPutDto
    {
        [Required]
        public string IdenNumber { get; set;}
    }

    public class ProviderWithListMedicinesDto : ProviderDto
    {
        public IEnumerable<MedicineBaseDto> MedicinesList{ get; set;}
    }

    public class ProviderWithTotalQuantityDto
    {
    public string Name { get; set; }
    public ICollection<MedicineWithQuantityDto> MedicinesList { get; set; }
    public int TotalPurchaseCant { get; set; }
    }
}