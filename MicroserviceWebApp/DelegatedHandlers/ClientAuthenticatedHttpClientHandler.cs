using Duende.IdentityModel.Client;
using MicroserviceWebApp.Options.IdentityOptions;
using MicroserviceWebApp.Services;

namespace MicroserviceWebApp.DelegatedHandlers
{
    internal class ClientAuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor,TokenService tokenService):DelegatingHandler
    {
         protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (httpContextAccessor.HttpContext is null) return await base.SendAsync(request, cancellationToken);
            if(httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated) return await base.SendAsync(request,cancellationToken);
            var tokenresponse= await tokenService.GetClientAccessToken();
            if (tokenresponse.IsError)
            {
                throw new UnauthorizedAccessException(tokenresponse.Error);
            }
            request.SetBearerToken(tokenresponse.AccessToken!);
            return await base.SendAsync(request, cancellationToken);

        }
    }
}
