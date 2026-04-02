using MediatR;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Payment.Api.Feature.Get
{
    public static class GetAllPaymentsByUserIdEndpoint
    {
        public static RouteGroupBuilder GetAllPaymentsByUserIdItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/",
                async ( IMediator mediator) =>
                    (await mediator.Send(new GetAllPaymentsByUserIdQuery())).ToGenericResult())
                .WithName("GetPayment")
                .MapToApiVersion(1, 0)
                .Produces(StatusCodes.Status200OK)
                .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
                .Produces<ProblemDetails>(StatusCodes.Status404NotFound).RequireAuthorization("ClientCredential");

            return groupBuilder;
        }
    }
}
