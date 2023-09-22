using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class PurchaseManyPostDto
{
    [Required]
    public int ProviderId { get; set;}
    [Required]
    public List<PurchaseMedicinePostDto> MedicinesList { get; set; }
}
