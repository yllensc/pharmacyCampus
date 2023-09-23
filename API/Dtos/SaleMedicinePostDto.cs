using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class SaleMedicinePostDto
{
    [Required]
    public int MedicineId {get; set;}
    [Required]
    public int SaleQuantity { get; set;}
    [Required]
    public double Price { get; set;}
}