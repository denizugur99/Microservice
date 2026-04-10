using Microservice.Catalog.Api.Features.Courses.Update;
using Microservices.Shared.Filter;
using Microservices.Shared.Extensions;

namespace Microservice.Catalog.Api.Features.Courses.Delete
{
    public record DeleteCourseCommand(Guid Id) : IrequestByServiceResult;

    public class DeleteCourseCommandHandler(AppDbContext context) : IRequestHandler<DeleteCourseCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course= await context.Courses.FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
            if (course is null) {
                return ServiceResult.ErrorAsNotFound();
            }
            context.Courses.Remove(course);
            await context.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccesAsNoContent();
        }
    }
    public static class DeleteCourseEndpoint
    {
        public static RouteGroupBuilder DeleteCourseItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) => (await mediator.Send(new DeleteCourseCommand(id))).ToGenericResult()).WithName("DeleteCourseById")
                .MapToApiVersion(1, 0).RequireAuthorization(policyNames:"InstructorPolicy");
                
            return groupBuilder;
        }
    }
}
