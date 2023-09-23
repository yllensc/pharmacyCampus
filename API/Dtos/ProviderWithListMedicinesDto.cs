using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProviderWithListMedicinesDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string IdenNumber { get; set;}
        [Required]
        public string Email { get; set;}
        [Required]
        public string Address { get; set;}
        public IEnumerable<MedicineDto> Medicines{ get; set;}
    }
}