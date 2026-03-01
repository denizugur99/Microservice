
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Reflection;

namespace Microservice.Discount.API.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        public DbSet<Discount.API.Features.Discount.Discount> Discounts { get; set; }=null!;
        public static AppDbContext Create(IMongoDatabase database)
        {
            var appDbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            return new AppDbContext(appDbContextOptions.Options);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
     
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
