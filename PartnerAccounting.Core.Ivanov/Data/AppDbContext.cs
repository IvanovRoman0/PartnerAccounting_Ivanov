using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PartnerAccounting.Core.Ivanov.Models;

namespace PartnerAccounting.Core.Ivanov.Data
{

    public class AppDbContext : DbContext
    {
        public DbSet<PartnerType> PartnerTypes { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleHistory> SalesHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=ivanov_db;Username=app;Password=123456789");

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }

            modelBuilder.Entity<SaleHistory>()
                .HasIndex(sh => new { sh.PartnerId, sh.SaleDate })
                .HasDatabaseName("ix_sales_partner_date");

            modelBuilder.Entity<Partner>()
                .HasOne(p => p.PartnerType)
                .WithMany(pt => pt.Partners)
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SaleHistory>()
                .HasOne(sh => sh.Partner)
                .WithMany(p => p.SalesHistory)
                .HasForeignKey(sh => sh.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleHistory>()
                .HasOne(sh => sh.Product)
                .WithMany(p => p.SalesHistory)
                .HasForeignKey(sh => sh.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .Property(p => p.MinPartnerPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SaleHistory>()
                .Property(sh => sh.TotalAmount)
                .HasPrecision(10, 2);
        }
    }
}