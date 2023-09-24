using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class SaleAverageDto
{
    public int SaleId { get; set; }
    public int MedicineId { get; set; }
    public int Average { get; set; }
}