using MediatR;
using Microservice.Basket.Api.Dto;
using Microservices.Shared.Extensions;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket
{
    public static class GetBasketEndpoint
    {
        public static RouteGroupBuilder GetBasketGroupEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/user",
                async (IMediator mediator) =>
                    (await mediator.Send(new GetBasketQuery())).ToGenericResult())
                .WithName("GetBasket")
                .MapToApiVersion(1, 0)
                .Produces<BasketDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
