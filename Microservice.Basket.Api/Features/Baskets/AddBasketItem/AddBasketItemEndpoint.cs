using Asp.Versioning.Builder;
using MediatR;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem
{
    public static class AddBasketItemEndpoint
    {
        public static RouteGroupBuilder AddBasketGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/item",
                async (AddBasketItemCommand command, IMediator mediator) => (await mediator.Send(command)).ToGenericResult()).WithName("Createbasket")
                .MapToApiVersion(1, 0)
                .Produces<Guid>(StatusCodes.Status201Created).Produces(StatusCodes.Status404NotFound).AddEndpointFilter<ValidationFilter<AddBasketItemCommand>>();

            return groupBuilder;
        }
    }
}
