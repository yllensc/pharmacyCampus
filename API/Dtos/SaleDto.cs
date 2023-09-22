using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class SaleDto
{
    public int Id { get; set; }
    [Required]
    public DateTime DateSale { get; set; } = DateTime.UtcNow;
    [Required]
    public int PatientId { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public bool Prescription { get; set; }
    [Required]
    public int MedicineId {get; set;}
    [Required]
    public int QuantitySale { get; set;}
    [Required]
    public double Price { get; set;}

}