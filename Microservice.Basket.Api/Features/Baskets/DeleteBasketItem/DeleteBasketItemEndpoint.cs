using MediatR;
using Microservice.Basket.Api.Features.Baskets.AddBasketItem;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem
{
    public static class DeleteBasketItemEndpoint
    {
        public static RouteGroupBuilder DeleteBasketGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapDelete("/item",
                async (DeleteBasketItemCommand command, IMediator mediator) => (await mediator.Send(command)).ToGenericResult()).WithName("DeleteBasket")
                .MapToApiVersion(1, 0)
                .AddEndpointFilter<ValidationFilter<DeleteBasketCommandValidator>>();

            return groupBuilder;
        }
    }
}
