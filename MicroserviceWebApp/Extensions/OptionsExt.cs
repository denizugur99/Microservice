using MicroserviceWebApp.Options.IdentityOptions;
using MicroserviceWebApp.Options.GatewayOptions;
using Microsoft.Extensions.Options;

namespace MicroserviceWebApp.Extensions
{
    public static class OptionsExt
    {
        public static IServiceCollection AddOptionsExt(this IServiceCollection services)
        {
            services.AddOptions<IdentityOption>().BindConfiguration(nameof(IdentityOption)).ValidateDataAnnotations().ValidateOnStart();
            services.AddSingleton<IdentityOption>(sp=>sp.GetRequiredService<IOptions<IdentityOption>>().Value);

            services.AddOptions<GatewayOption>().BindConfiguration(nameof(GatewayOption)).ValidateDataAnnotations().ValidateOnStart();
            services.AddSingleton<GatewayOption>(sp=>sp.GetRequiredService<IOptions<GatewayOption>>().Value);

            return services;
        }
    }
}
