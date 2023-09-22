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
<<<<<<< HEAD
        builder.Property(p=> p.Id)
        .IsRequired();
=======
>>>>>>> c89bc5f (Adaptación de las tablas "intermedias" jeje)
        builder.HasOne(m => m.Purchase)
        .WithMany(m => m.PurchasedMedicines)
        .HasForeignKey(m => m.PurchasedId)
        .IsRequired();
        builder.HasOne(m => m.Medicine)
        .WithMany(m => m.PurchasedMedicines)
        .HasForeignKey(m => m.MedicineId)
        .IsRequired();
        builder.Property(p => p.CantPurchased)
        .IsRequired();
        builder.Property(p => p.PricePurchase)
        .IsRequired();
        builder.Property(p => p.ExpirationDate)
        .IsRequired()
        .HasColumnType("DateTime");
    }
}