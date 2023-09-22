namespace Domain.Entities;

public class Medicine : BaseEntity
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
    public int ProviderId { get; set; } 
    public Provider Provider{ get; set; }
    public ICollection<PurchasedMedicine> PurchasedMedicines{ get; set; }
    public ICollection<SaleMedicine> SaleMedicines{ get; set; }
}