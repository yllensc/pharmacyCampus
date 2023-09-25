using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MedicinePutDto
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public double Price { get; set; }

    }
    public class MedicineBaseDto : MedicinePutDto
    {
        [Required]
        public string Name { get; set; }
        
    }
        public class MedicineWithQuantityDto
    {
        [Required]
        public string Name { get; set; }
        public int PurchaseCant { get; set; }
        
    }
    public class MedicineWithStockDto
    {
        [Required]
        public string Name { get; set; }
        public int Stock { get; set; }
    }

    public class MedicineAllDto : MedicineBaseDto
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }

    }
}