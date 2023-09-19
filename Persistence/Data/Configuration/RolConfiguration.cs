using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("rol");
        builder.Property(p => p.Name)
        .HasColumnType("varchar(255) COLLATE utf8mb4_unicode_ci")
        .HasMaxLength(50)
        .IsRequired();

    }
}
