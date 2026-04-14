using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Bus.Events
{
    public record DiscountNotificationEvent(string Email, string Code);
    
}
