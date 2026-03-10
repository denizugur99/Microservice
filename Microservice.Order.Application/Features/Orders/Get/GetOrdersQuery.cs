using Microservices.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Features.Orders.Get
{
    public record GetOrdersQuery : IrequestByServiceResult<List<GetOrdersResponse>>;
   

}
