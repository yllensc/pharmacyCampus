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
        builder.HasOne(d => d.Provider)
        .WithMany(p => p.Medicines)
        .HasForeignKey(d => d.ProviderId);
        builder.HasIndex(m => new { m.Name, m.ProviderId })
        .IsUnique();
    }
}