using MediatR;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Payment.Api.Feature.Payments.Create
{
    public static class GetAllPaymentsByUserIdEndpoint
    {
        public static RouteGroupBuilder CreatePaymentGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/",
                async ([FromBody] CreatePaymentCommand command, [FromServices] IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
                .WithName("CreatePayment")
                .MapToApiVersion(1, 0)
                .AddEndpointFilter<ValidationFilter<CreatePaymentCommand>>()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
