
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

public class SaleMedicineConfiguration : IEntityTypeConfiguration<SaleMedicine>
{
    public void Configure(EntityTypeBuilder<SaleMedicine> builder)
    {
        builder.ToTable("saleMedicine");
        builder.HasKey(m => new { m.SaleId, m.MedicineId }); // Definir clave primaria compuesta
        builder.Property(p => p.SaleQuantity)
        .IsRequired()
        .HasColumnType("int");
        builder.Property(p => p.Price)
        .IsRequired()
        .HasColumnType("decimal");
        builder.HasOne(d => d.Sale)
        .WithMany(p => p.SaleMedicines)
        .HasForeignKey(d => d.SaleId);
        builder.HasOne(d => d.Medicine)
        .WithMany(p => p.SaleMedicines)
        .HasForeignKey(d => d.MedicineId);
    }
}