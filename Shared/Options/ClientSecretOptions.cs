using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Shared.Options
{
   public class ClientSecretOptions
    {
        public required string Id { get; set; }
        public required string Secret { get; set; }
    }
}
