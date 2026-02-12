using MediatR;
using Microservices.Shared.Extensions;
using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Catalog.Api.Features.Categories.Create
{
    public static class CreateCategoryEndpoint
    {
        public static RouteGroupBuilder CreateCategoryGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/",
                async (CreateCategoryCommand command, IMediator mediator) => (await mediator.Send(command)).ToGenericResult()).AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>();
          
            return groupBuilder;
        }
    }
}
