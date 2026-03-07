using Microservice.Order.Domain.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        DbSet<Domain.Entitites.Order> Orders { get; set; }
        DbSet<Domain.Entitites.OrderItem> OrderItems { get; set; }
        DbSet<Address> Addresses { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);

            // OrderItem configuration - use Guid property as FK
            modelBuilder.Entity<Domain.Entitites.OrderItem>()
                .Property<Guid>("OrderId")
                .HasColumnName("OrderId");

            // Fix cascade delete paths for SQL Server
            modelBuilder.Entity<Domain.Entitites.Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Domain.Entitites.Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
