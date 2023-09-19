using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Data.Configuration;
public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("purchase");
        builder.Property(p => p.DatePurchase)
        .IsRequired();
        builder.HasOne(b => b.Provider)
        .WithMany(b => b.Purchases)
        .HasForeignKey(b => b.ProviderId)
        .IsRequired();


    }
}
