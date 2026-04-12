using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MicroserviceWebApp.Services
{
    public class UserService(IHttpContextAccessor httpContextAccessor) 
    {
        public Guid GetUserId { get
            {
                if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new Exception("User is not authenticated");
                }
                return Guid.Parse(httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type =="sub")?.Value!);
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
