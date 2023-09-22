using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
    public class LotConfiguration: IEntityTypeConfiguration<Lot>
{
    public void Configure(EntityTypeBuilder<Lot> builder)
    {
        
        builder.ToTable("lot");
        builder.Property(p => p.ExpireDate)
            .IsRequired(); 
        builder.Property(p => p.Price)
            .HasColumnType("decimal")
            .IsRequired();
        builder.Property(p => p.Stock)
            .IsRequired()
            .HasColumnType("int");
        builder.HasOne(p => p.Medicine)
            .WithMany(m => m.Lots)
            .HasForeignKey(p => p.MedicineId);
        builder.HasOne(p => p.Provider)
            .WithMany(p => p.Lots)
            .HasForeignKey(p => p.ProviderId);
    }
}