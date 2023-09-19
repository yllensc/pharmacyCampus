namespace Domain.Entities;

public class SoldMedicine : BaseEntity
{
    public int SoldId { get; set; }
    public Sale Sale {get; set;}
    public int MedicineId { get; set; }
    public Medicine Medicine { get; set;}
    public int SoldQuantity { get; set; }
    public double Price { get; set; }
}