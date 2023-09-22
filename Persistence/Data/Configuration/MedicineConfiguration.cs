using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;

public class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.ToTable("medicine");

        builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(p => p.Price)
        .HasColumnType("decimal")
        .IsRequired();

        builder.Property(p => p.Stock)
        .IsRequired()
        .HasColumnType("int");

        builder.Property(p => p.ExpirationDate)
        .IsRequired()
        .HasColumnType("DateTime");
        
        builder.HasOne(d => d.Provider)
        .WithMany(p => p.Medicines)
        .HasForeignKey(d => d.ProviderId);
    }
}