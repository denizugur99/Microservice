using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Microservices.Shared.Services
{
    internal class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
    {
        public Guid GetUserId { get
            {
                if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new Exception("User is not authenticated");
                }
                return Guid.Parse(httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
            } }
        public string GetUserName
        {
            get
            {
                if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
                {
                    throw new Exception("User is not authenticated");
                }
                return httpContextAccessor.HttpContext!.User.Identity!.Name!;
            }
        }
        public List<string> Roles
        {
            get
            {
                if (!httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
                {
                    throw new Exception("User is not authenticated");
                }
                return httpContextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            }
        }
    }
}
