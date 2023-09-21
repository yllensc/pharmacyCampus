using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
    public class PositionConfiguration: IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("position");
        builder.Property(p => p.Name)
        .HasMaxLength(50)
        .IsRequired();
        builder.HasIndex(p => p.Name)
        .IsUnique();
        
    }
}