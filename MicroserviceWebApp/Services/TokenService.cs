using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroserviceWebApp.Services
{
    public class TokenService
    {
        public List<Claim>ExtractClaims(string accresToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims= handler.ReadJwtToken(accresToken).Claims.ToList();
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
                IsPersistent= true
            };
           
            authenticationProperties.StoreTokens(authenticationTokens);
            return authenticationProperties;
        }
    }
}
