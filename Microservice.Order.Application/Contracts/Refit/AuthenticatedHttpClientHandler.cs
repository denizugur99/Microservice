using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Contracts.Refit
{
    public class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContext):DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(httpContext.HttpContext==null) return await base.SendAsync(request, cancellationToken);

            if (!httpContext.HttpContext!.User.Identity!.IsAuthenticated) return await base.SendAsync(request, cancellationToken);
            string token=string.Empty;

            if(httpContext.HttpContext.Request.Headers.TryGetValue("Authorization",out var authHeader))
            {
                token = authHeader.ToString().Split(" ")[1];
            }
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
