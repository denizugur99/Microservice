using MediatR;
using Microservice.Order.Application.Features.Orders.Get;
using Microservices.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Order.Api.Endpoints.Orders
{
    public static class GetOrdersEndpoint
    {
        public static RouteGroupBuilder GetOrdersGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/",
                async ([FromServices] IMediator mediator) => (await mediator.Send(new GetOrdersQuery())).ToGenericResult())
                .WithName("GetOrders")
                .MapToApiVersion(1, 0)
                .Produces<List<GetOrdersResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
