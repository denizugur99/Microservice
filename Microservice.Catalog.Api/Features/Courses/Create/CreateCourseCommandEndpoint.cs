using Microservices.Shared.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public static class CreateCourseCommandEndpoint
    {
        public static RouteGroupBuilder CreateCourseEndpoint(this RouteGroupBuilder groupBuilder)
        {
           groupBuilder.MapPost("/",async([FromForm]CreateCourseCommand command,IMediator mediator)=>(await mediator.Send(command)).ToGenericResult()).WithName("createCourse")
                .MapToApiVersion(1, 0)
                .Produces<Guid>(StatusCodes.Status201Created).Produces(StatusCodes.Status404NotFound).Produces<ProblemDetails>(StatusCodes.Status400BadRequest).AddEndpointFilter<ValidationFilter<CreateCourseCommand>>().DisableAntiforgery().RequireAuthorization(policyNames:"InstructorPolicy");
            return groupBuilder;
        }
    }
}
