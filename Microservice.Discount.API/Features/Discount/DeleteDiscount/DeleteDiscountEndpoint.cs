using Microsoft.AspNetCore.Mvc;

namespace Microservice.Discount.API.Features.Discount.DeleteDiscount
{
    public static class DeleteDiscountEndpoint
    {
        public static RouteGroupBuilder DeleteDiscountItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapDelete("/{code:length(10)}",
                async (string code, IMediator mediator) =>
                    (await mediator.Send(new DeleteDiscountCommand(code))).ToGenericResult())
                .WithName("deleteDiscount")
                .MapToApiVersion(1, 0)
                .Produces(StatusCodes.Status204NoContent)
                .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
