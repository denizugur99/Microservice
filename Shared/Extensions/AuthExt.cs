using Microservices.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Microservices.Shared.Extensions
{
    public static class AuthExt
    {
        public static IServiceCollection AddAuthenticationExt(this IServiceCollection services, IConfiguration configuration) {
            var identityOption= configuration.GetSection(nameof(IdentityOption)).Get<IdentityOption>();
            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = identityOption.Address;
                options.Audience = identityOption.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    RoleClaimType=ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name

                };

            }).AddJwtBearer("ClientCredentialSchema", options =>
            {
                options.Authority = identityOption.Address;
                options.Audience = identityOption.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true

                };

            });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("InstructorPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Email);
                    policy.RequireRole(ClaimTypes.Role,"instructor"); 
                });
                opt.AddPolicy("ClientCredential", policy =>
                {
                    policy.AddAuthenticationSchemes("ClientCredentialSchema");
                    policy.RequireAuthenticatedUser();
                   
                });
                opt.AddPolicy("Password", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Email);
                });
            });
            //sign
            //aud=>payment api
            //issuer=>localhost:8080
            //tokenlifetime 

            return services;
        }
    }
}
