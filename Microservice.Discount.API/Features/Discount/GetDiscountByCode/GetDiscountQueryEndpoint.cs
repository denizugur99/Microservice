using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Discount.API.Features.Discount.GetDiscountByCode
{
    public static class GetDiscountQueryEndpoint
    {
        public static RouteGroupBuilder GetDiscountEndpoint(this RouteGroupBuilder groupBuilder)
        {
           groupBuilder.MapGet("/{code:length(10)}",async(string code, IMediator mediator)=>(await mediator.Send(new GetDiscountByCodeQuery(code))).ToGenericResult()).WithName("getDiscount")
                .MapToApiVersion(1, 0);
            return groupBuilder;
        }
    }
}
