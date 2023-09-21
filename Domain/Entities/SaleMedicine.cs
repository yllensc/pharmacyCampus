namespace Domain.Entities;

public class SaleMedicine 
{
    public int SaleId { get; set; }
    public Sale Sale {get; set;}
    public int MedicineId { get; set; }
    public Medicine Medicine { get; set;}
    public int SaleQuantity { get; set; }
    public double Price { get; set; }
}