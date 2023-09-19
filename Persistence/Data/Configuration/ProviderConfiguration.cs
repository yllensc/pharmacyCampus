using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("provider");
        builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(150);
        builder.Property(p => p.IdenNumber)
        .IsRequired()
        .HasMaxLength(15);
        builder.HasIndex(p => p.IdenNumber)
        .IsUnique();
        builder.Property(p => p.Email)
        .IsRequired();
        builder.Property(p => p.Address)
        .IsRequired();

        
    }
}
