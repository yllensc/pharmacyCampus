using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class SaleDto
{
    [Required]
    public int PatientId { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public bool Prescription { get; set; }
    [Required]
    public int MedicineId {get; set;}
    [Required]
    public int SaleQuantity { get; set;}

}