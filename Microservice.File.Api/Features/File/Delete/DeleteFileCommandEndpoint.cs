using MediatR;
using Microservice.File.Api.Features.File.Delete;
using Microservices.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Discount.API.Features.Discount.DeleteDiscount
{
    public static class DeleteFileCommandEndpoint
    {
        public static RouteGroupBuilder DeleteFileEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapDelete("/", async ([FromBody]DeleteFileCommand command,[FromServices] IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
                .WithName("deleteFile")
                .MapToApiVersion(1, 0)
                .Produces(StatusCodes.Status204NoContent)
                .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

            return groupBuilder;
        }
    }
}
