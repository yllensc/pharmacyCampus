using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class PatientDto
{
    [Required]
    public string IdenNumber { get; set;}
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address   { get; set; }
    [Required]
    public string PhoneNumber { get; set; }   
}

public class PatientOnlyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class PatientSpentDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double TotalSpent { get; set; }
}
