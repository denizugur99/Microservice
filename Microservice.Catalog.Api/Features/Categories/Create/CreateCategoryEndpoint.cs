using Asp.Versioning.Builder;
using Microservices.Shared.Filter;

namespace Microservice.Catalog.Api.Features.Categories.Create
{
    public static class CreateCategoryEndpoint
    {
        public static RouteGroupBuilder CreateCategoryGroupItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapPost("/",
                async (CreateCategoryCommand command, IMediator mediator) => (await mediator.Send(command)).ToGenericResult()).WithName("Createcategory")
                .MapToApiVersion(1,0)
                .Produces<Guid>(StatusCodes.Status201Created).Produces(StatusCodes.Status404NotFound).AddEndpointFilter<ValidationFilter<CreateCategoryCommand>>().RequireAuthorization("Password");
          
            return groupBuilder;
        }
    }
}
