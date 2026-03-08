using MediatR;
using Microservice.Order.Application.Features.Orders.Create;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Order.Api.Endpoints.Orders
{
    public static class CreateOrderEndpoint
    {
        public static RouteGroupBuilder CreateOrderGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/",
                async ([FromBody]CreateOrderComand command,[FromServices] IMediator mediator) => (await mediator.Send(command)).ToGenericResult())
                .WithName("Createorder")
                .MapToApiVersion(1, 0)
                .AddEndpointFilter<ValidationFilter<CreateOrderComand>>()
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
