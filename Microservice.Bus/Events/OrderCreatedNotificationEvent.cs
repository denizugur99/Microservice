using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Bus.Events
{
    public record OrderCreatedNotificationEvent(string Email, Guid OrderId);
    
}
