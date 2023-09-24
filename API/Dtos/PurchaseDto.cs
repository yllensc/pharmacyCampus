using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

public class PurchaseDto
{
    public int Id { get; set; }
    public DateTime DatePurchase { get; set; }
    public int ProviderId { get; set; }
    public List<PurchaseMedicineDto> purchaseMedicines {get; set; }
    
}
