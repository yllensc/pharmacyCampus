using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<Patient> Patients{ get; set; }
        public DbSet<Medicine> Medicines{ get; set; }
        public DbSet<PurchasedMedicine> PurchasedMedicines{ get; set; }
        public DbSet<Provider> Providers{ get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleMedicine> SaleMedicines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UserRol> UserRoles { get; set; }

        
        
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}