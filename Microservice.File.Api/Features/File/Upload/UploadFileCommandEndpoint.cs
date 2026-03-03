using MediatR;
using Microservice.File.Api.Features.File.Upload;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Discount.API.Features.Discount.CreateDiscount
{
    public static class UploadFileCommandEndpoint
    {
        public static RouteGroupBuilder UploadFileEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/", async (IFormFile file, IMediator mediator) => (await mediator.Send(new UploadFileCommand(file))).ToGenericResult()).WithName("uploadFile")
                 .MapToApiVersion(1, 0)
                 .Produces<Guid>(StatusCodes.Status201Created).Produces<ProblemDetails>(StatusCodes.Status400BadRequest).DisableAntiforgery();
            return groupBuilder;
        }
    }
}
