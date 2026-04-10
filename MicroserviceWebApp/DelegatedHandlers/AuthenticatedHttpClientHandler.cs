using Duende.IdentityModel.Client;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

namespace MicroserviceWebApp.DelegatedHandlers
{
    public class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor,TokenService tokenService):DelegatingHandler
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

            request.SetBearerToken(tokenresponse.AccessToken!);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
