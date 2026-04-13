using Duende.IdentityModel.Client;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Security.Claims;

namespace MicroserviceWebApp.DelegatedHandlers
{
    internal class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor,TokenService tokenService):DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(httpContextAccessor.HttpContext is null) return await base.SendAsync(request, cancellationToken);

            var user = httpContextAccessor.HttpContext.User;
            if(user ==null || !user.Identity.IsAuthenticated) return await base.SendAsync(request, cancellationToken);

            var accessToken= await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if(string.IsNullOrEmpty(accessToken)) throw new UnauthorizedAccessException("No access token available");

            request.SetBearerToken(accessToken);

            var response= await base.SendAsync(request, cancellationToken);
            if(response.StatusCode!=HttpStatusCode.Unauthorized) return response;
            var refreshToken= await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            if(string.IsNullOrEmpty(refreshToken)) throw new UnauthorizedAccessException("No refresh token available");
            var tokenresponse=await tokenService.GetTokenByRefreshToken(refreshToken);
            if(tokenresponse.IsError) throw new UnauthorizedAccessException("Failed to refresh access token");

            //todo : update the authentication cookie with new tokens
            var authenticationProperties= tokenService.CreateAuthenticationProperties(tokenresponse);
            var userClaims=httpContextAccessor.HttpContext.User.Claims;
            var claimIdentity= new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme,ClaimTypes.Name,ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
            request.SetBearerToken(tokenresponse.AccessToken!);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
