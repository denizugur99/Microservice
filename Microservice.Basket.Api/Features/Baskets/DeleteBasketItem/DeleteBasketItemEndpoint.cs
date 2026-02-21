using MediatR;
using Microservice.Basket.Api.Features.Baskets.AddBasketItem;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem
{
    public static class DeleteBasketItemEndpoint
    {
        public static RouteGroupBuilder DeleteBasketGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapDelete("/item/{Id:guid}",
                async (Guid Id, IMediator mediator) => (await mediator.Send(new DeleteBasketItemCommand(Id))).ToGenericResult()).WithName("DeleteBasket")
                .MapToApiVersion(1, 0)
                .AddEndpointFilter<ValidationFilter<DeleteBasketItemCommand>>();

            return groupBuilder;
        }
    }
}
