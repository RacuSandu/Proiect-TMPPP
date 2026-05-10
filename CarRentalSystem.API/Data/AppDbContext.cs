using Microsoft.EntityFrameworkCore;
using CarRentalSystem.Models;

namespace CarRentalSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<VehicleEntity> Vehicles { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vehicule
            modelBuilder.Entity<VehicleEntity>(e =>
            {
                e.HasKey(v => v.Id);
                e.Property(v => v.DailyRate).HasColumnType("decimal(18,2)");
            });

            // Clienti
            modelBuilder.Entity<CustomerEntity>(e =>
            {
                e.HasKey(c => c.Id);
            });

            // Contracte
            modelBuilder.Entity<ContractEntity>(e =>
            {
                e.HasKey(c => c.ContractId);
                e.Property(c => c.TotalCost).HasColumnType("decimal(18,2)");
            });

            // Plati
            modelBuilder.Entity<PaymentEntity>(e =>
            {
                e.HasKey(p => p.PaymentId);
                e.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            });
        }
    }
}