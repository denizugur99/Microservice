using Microservice.Order.Application.Features.Orders.Create;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Features.Orders.Get
{
    public record GetOrdersResponse(DateTimeOffset OrderDate, decimal TotalPrice, List<OrderItemDto> OrderItems);
   
    
}
