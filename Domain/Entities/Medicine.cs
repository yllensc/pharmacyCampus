namespace Domain.Entities;

public class Medicine : BaseEntity
{
    public string Name { get; set; }
    public int ProviderId { get; set; } 
    public Provider Provider{ get; set; }
    public ICollection<Lot> Lots { get; set; }
    public ICollection<PurchasedMedicine> PurchasedMedicines{ get; set; }
    public ICollection<SaleMedicine> SaleMedicines{ get; set; }
}