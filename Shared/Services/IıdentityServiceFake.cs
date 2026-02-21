using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.Services
{
    public class IıdentityServiceFake : IIdentityService
    {
        public Guid GetUserId =>Guid.Parse("898688f4-de46-415f-a393-9e73b8fb3bd1");

        public string GetUserName => "Ahmet16";
    }
}
