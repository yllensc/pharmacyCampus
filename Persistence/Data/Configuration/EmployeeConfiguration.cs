using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employee");
        builder.Property(p => p.Id)
        .IsRequired();
        builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(100);
        builder.Property(p => p.IdenNumber)
        .IsRequired()
        .HasMaxLength(15);
        builder.HasIndex(p => p.IdenNumber)
        .IsUnique();
        builder.Property(p => p.Position)
        .IsRequired()
        .HasMaxLength(50);
        builder.Property(p=> p.DateContract)    
        .IsRequired();
    }
}
