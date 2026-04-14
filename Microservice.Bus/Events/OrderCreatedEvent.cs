using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Bus.Events
{
    public record OrderCreatedEvent(Guid OrderId, Guid UserId,string Email);
    
}
