using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Microservice.Discount.API.Options
{
    public static class OptionExt
    {
        public static IServiceCollection AddOptionsExt(this IServiceCollection services)
        {
            services.AddOptions<MongoOptions>().BindConfiguration(nameof(MongoOptions)).ValidateDataAnnotations().ValidateOnStart();
            services.AddSingleton(sp=>sp.GetRequiredService<IOptions<MongoOptions>>().Value);
            return services;
        }
    }
}
