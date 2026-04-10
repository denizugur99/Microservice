using Duende.IdentityModel.Client;
using MicroserviceWebApp.Options.IdentityOptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroserviceWebApp.Services
{
    public class TokenService(IdentityOption identityOption, HttpClient httpClient)
    {
        public List<Claim> ExtractClaims(string accresToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims = handler.ReadJwtToken(accresToken).Claims.ToList();
            return claims;

        }
        public AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse)
        {
            var authenticationTokens = new List<AuthenticationToken>
            {
                new()
                {
                    Name=OpenIdConnectParameterNames.AccessToken,
                    Value=tokenResponse.AccessToken
                },
                new()
                {
                    Name=OpenIdConnectParameterNames.RefreshToken,
                    Value=tokenResponse.RefreshToken
                },
                new()
                {
                    Name=OpenIdConnectParameterNames.ExpiresIn,
                    Value=DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o") // ISO 8601 format
                }

            };
            AuthenticationProperties authenticationProperties = new()
            {
                IsPersistent = true
            };

            authenticationProperties.StoreTokens(authenticationTokens);
            return authenticationProperties;
        }

        public async Task<TokenResponse> GetTokenByRefreshToken(string refreshToken)
        {
            var discoveryRequest = new DiscoveryDocumentRequest()
            {
                Address = identityOption.Address,
                Policy =
                    {
                        RequireHttps=false
                    }
            };
            var client = httpClient;
            client.BaseAddress = new Uri(identityOption.Address);
            var discoveryResponse = await client.GetDiscoveryDocumentAsync();
            if (discoveryResponse.IsError)
            {
                throw new Exception($"Identity Server Discovery Failed:{discoveryResponse.Error}");
            }
            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identityOption.Web.ClientId,
                ClientSecret = identityOption.Web.ClientSecret,
                RefreshToken = refreshToken
            });
            return tokenResponse;


        }

        public async Task<TokenResponse> GetClientAccessToken()
        {

            var discoveryRequest = new DiscoveryDocumentRequest()
            {
                Address = identityOption.Address,
                Policy =
                {
                    RequireHttps = false
                }

            };
            httpClient.BaseAddress = new Uri(identityOption.Address);
            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(discoveryRequest);
            if (discoveryResponse.IsError)
            {
                throw new Exception($"Identity Server Discovery Failed:{discoveryResponse.Error}");
            }
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identityOption.Web.ClientId,
                ClientSecret = identityOption.Web.ClientSecret,

            });
            if (tokenResponse.IsError)
            {
                throw new Exception($"Token Request Failed:{tokenResponse.Error}");
            }
            return tokenResponse;
        }
    }
}