using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.Services
{
   public interface IIdentityService
    {
        public Guid GetUserId {  get; }
        public string GetUserName { get; }
        public List<string> Roles => [];
    }
}
