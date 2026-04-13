using FluentValidation;
using FluentValidation.AspNetCore;
using Microservices.Shared.ExceptionHandler;
using Microservices.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.Extensions
{
    public static class CommonServiceExt
    {
        public static IServiceCollection AddCommonServiceExt(this IServiceCollection services,Type assembly)
        {
            services.AddHttpContextAccessor();
            services.AddMediatR(x=>x.RegisterServicesFromAssemblyContaining(assembly));
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(assembly);
            services.AddAutoMapper(x=>x.AddMaps(assembly));
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
