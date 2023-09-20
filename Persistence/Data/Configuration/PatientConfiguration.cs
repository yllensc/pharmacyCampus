using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patient");
        builder.Property(p=> p.Id)
        .IsRequired();
        builder.Property(p => p.Name)
        .IsRequired()
        .HasMaxLength(100);
        builder.Property(p => p.Address)
        .IsRequired()
        .HasMaxLength(50); 
        builder.Property(p => p.PhoneNumber)
        .IsRequired()
        .HasMaxLength(25);
    }
}
