using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class PurchasedMedicineConfiguration : IEntityTypeConfiguration<PurchasedMedicine>
{
    public void Configure(EntityTypeBuilder<PurchasedMedicine> builder)
    {
        builder.ToTable("purchasedMedicine");
        //builder.HasKey(m => new { m.PurchasedId, m.MedicineId }); // Definir clave primaria compuesta
        builder.Property(p=> p.Id)
        .IsRequired();
        builder.HasOne(m => m.Purchase)
        .WithMany(m => m.PurchasedMedicines)
        .HasForeignKey(m => m.PurchasedId);
        builder.HasOne(m => m.Medicine)
        .WithMany(m => m.PurchasedMedicines)
        .HasForeignKey(m => m.MedicineId);
        builder.Property(p => p.CantPurchased)
        .IsRequired();
        builder.Property(p => p.PricePurchase)
        .IsRequired();
        builder.Property(p => p.ExpirationDate)
        .IsRequired()
        .HasColumnType("DateTime");
    }
}