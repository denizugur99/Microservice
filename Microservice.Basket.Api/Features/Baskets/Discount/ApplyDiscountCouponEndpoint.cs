using MediatR;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;

namespace Microservice.Basket.Api.Features.Baskets.Discount
{
    public static class ApplyDiscountCouponEndpoint
    {
        public static RouteGroupBuilder ApplyDiscountCouponGroupEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPut("/discount",
                async (ApplyDiscountCouponCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
                .WithName("ApplyDiscountCoupon")
                .MapToApiVersion(1, 0)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .AddEndpointFilter<ValidationFilter<ApplyDiscountCouponCommand>>();

            return groupBuilder;
        }
    }
}
