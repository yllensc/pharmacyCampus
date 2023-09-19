using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("sale");
        builder.Property(p=> p.DateSale)
        .IsRequired();
        builder.HasOne(p=> p.Employee)
        .WithMany(p=> p.Sales)
        .HasForeignKey(p=>p.EmployeeId)
        .IsRequired();
        builder.HasOne(p=>p.Patient)
        .WithMany(p=> p.Sales)
        .HasForeignKey(p=>p.PatientId)
        .IsRequired();
    }
}
