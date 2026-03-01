using Microservice.Discount.API.Options;
using MongoDB.Driver;

namespace Microservice.Discount.API.Repositories
{
    public static class RepositoryExt
    {
        public static IServiceCollection AddDatabaseExt(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var mongoOptions = sp.GetRequiredService<MongoOptions>();
                return new MongoClient(mongoOptions.ConnectionString);

            });
           services.AddScoped(sp =>
            {
                var mongoOptions = sp.GetRequiredService<MongoOptions>();
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                return AppDbContext.Create(mongoClient.GetDatabase(mongoOptions.DatabaseName));
            });
            return services;
        }
    }
}
