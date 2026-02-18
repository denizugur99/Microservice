using Microservice.Catalog.Api.Features.Courses.Create;
using Microservices.Shared.Filter;

namespace Microservice.Catalog.Api.Features.Courses.Update
{
    public static class UpdateCoursCommandEndpoint
    {
       
            public static RouteGroupBuilder UpdateCourseEndpoint(this RouteGroupBuilder groupBuilder)
            {
                groupBuilder.MapPut("/", async (UpdateCourseCommand command, IMediator mediator) => (await mediator.Send(command)).ToGenericResult()).WithName("updateCourse")
                     .MapToApiVersion(1, 0)
                     .AddEndpointFilter<ValidationFilter<UpdateCourseCommand>>();
                return groupBuilder;
            }
        
    }
}
