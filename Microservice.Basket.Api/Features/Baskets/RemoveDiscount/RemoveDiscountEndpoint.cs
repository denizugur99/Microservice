using MediatR;
using Microservices.Shared.Extensions;

namespace Microservice.Basket.Api.Features.Baskets.RemoveDiscount
{
    public static class RemoveDiscountEndpoint
    {
        public static RouteGroupBuilder RemoveDiscountGroupEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapDelete("/discount",
                async (IMediator mediator) =>
                    (await mediator.Send(new RemoveDiscountCommand())).ToGenericResult())
                .WithName("RemoveDiscount")
                .MapToApiVersion(1, 0)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
