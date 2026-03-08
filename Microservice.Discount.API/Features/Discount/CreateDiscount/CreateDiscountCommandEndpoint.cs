using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Discount.API.Features.Discount.CreateDiscount
{
    public static class GetDiscountQueryEndpoint
    {
        public static RouteGroupBuilder CreateDiscountEndpoint(this RouteGroupBuilder groupBuilder)
        {
           groupBuilder.MapPost("/",async(CreateDiscountCommand command,IMediator mediator)=>(await mediator.Send(command)).ToGenericResult()).WithName("createDiscount")
                .MapToApiVersion(1, 0)
                .Produces<Guid>(StatusCodes.Status201Created).Produces<ProblemDetails>(StatusCodes.Status400BadRequest).AddEndpointFilter<ValidationFilter<CreateDiscountCommand>>();
            return groupBuilder;
        }
    }
}
