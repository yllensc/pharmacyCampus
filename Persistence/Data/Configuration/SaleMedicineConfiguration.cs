
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

public class SaleMedicineConfiguration : IEntityTypeConfiguration<SoldMedicine>
{
    public void Configure(EntityTypeBuilder<SoldMedicine> builder)
    {
        builder.ToTable("soldMedicine");
        builder.HasKey(m => new { m.SoldId, m.MedicineId }); // Definir clave primaria compuesta
        builder.Property(p => p.SoldQuantity)
        .IsRequired()
        .HasColumnType("int");
        builder.Property(p => p.Price)
        .IsRequired()
        .HasColumnType("decimal");
        builder.HasOne(d => d.Sale)
        .WithMany(p => p.SoldMedicines)
        .HasForeignKey(d => d.SoldId);
        builder.HasOne(d => d.Medicine)
        .WithMany(p => p.SoldMedicines)
        .HasForeignKey(d => d.MedicineId);
    }
}