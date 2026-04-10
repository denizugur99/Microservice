using MicroserviceWebApp.Options.IdentityOptions;
using MicroserviceWebApp.Options.GatewayOptions;
using Microsoft.Extensions.Options;
using MicroserviceWebApp.Options;

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

            services.AddOptions<MicroserviceOptions>().BindConfiguration(nameof(MicroserviceOptions)).ValidateDataAnnotations().ValidateOnStart();
            services.AddSingleton<MicroserviceOptions>(sp => sp.GetRequiredService<IOptions<MicroserviceOptions>>().Value);

            return services;
        }
    }
}
