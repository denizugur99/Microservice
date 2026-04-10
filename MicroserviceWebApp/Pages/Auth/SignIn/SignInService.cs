using Duende.IdentityModel.Client;
using MicroserviceWebApp.Options.IdentityOptions;
using MicroserviceWebApp.Pages.Auth.SignUp;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MicroserviceWebApp.Pages.Auth.SignIn
{
    public class SignInService(IHttpContextAccessor httpContextAccessor,TokenService tokenService, IdentityOption identityOption, HttpClient httpClient, ILogger<SignInService> logger)
    {
        public async Task<ServiceResult> SignInAsync(SignInViewModel signinVievModel)
        {
            var tokenResponse = await GetAccesToken(signinVievModel);
            if(tokenResponse.IsError)
            {
                logger.LogError("Failed to get access token for user {Email}. Error: {Error}", signinVievModel.Email, tokenResponse.Error);
                return ServiceResult.Error(tokenResponse.Error!,tokenResponse.ErrorDescription!);
            }

            var userClaims = tokenService.ExtractClaims(tokenResponse.AccessToken!);
            var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse);
            var claimIdentity= new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme,ClaimTypes.Name,ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);





            return ServiceResult.Success();
        }
       








        private async Task<TokenResponse> GetAccesToken(SignInViewModel signinVievModel)
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
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identityOption.Web.ClientId,
                ClientSecret=identityOption.Web.ClientSecret,
                UserName = signinVievModel.Email,
                Password = signinVievModel.Password
            });
          
            return tokenResponse;
        }
    }
}
