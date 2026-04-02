using Duende.IdentityModel.Client;
using Microservices.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Contracts.Refit
{
    public class ClientAuthenticatedHttpClientHandler(IServiceProvider serviceProvider,IHttpClientFactory httpClientFactory): DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.Headers.Authorization!=null) return await base.SendAsync(request, cancellationToken);
            using(var scope= serviceProvider.CreateScope())
            {
                var identityOptions=scope.ServiceProvider.GetRequiredService<IdentityOption>();
                var clientSecretOptions=scope.ServiceProvider.GetRequiredService<ClientSecretOptions>();
                var discoveryRequest= new DiscoveryDocumentRequest
                {
                    Address=identityOptions.Address,
                    Policy=
                    {
                        RequireHttps=false
                    }
                };
                var client=httpClientFactory.CreateClient();
                client.BaseAddress=new Uri(identityOptions.Address);
                var discoveryResponse=await client.GetDiscoveryDocumentAsync();
                if(discoveryResponse.IsError)
                {
                    throw new Exception($"Identity Server Discovery Failed:{discoveryResponse.Error}");
                }
                var tokenResponse=await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address=discoveryResponse.TokenEndpoint,
                    ClientId=clientSecretOptions.Id,
                    ClientSecret=clientSecretOptions.Secret, 
                });
                if(tokenResponse.IsError)
                {
                    throw new Exception($"Token Request Failed:{tokenResponse.Error}");
                }
                request.SetBearerToken(tokenResponse.AccessToken!);
                return await base.SendAsync(request, cancellationToken);
            }
           
        }
    }
}
