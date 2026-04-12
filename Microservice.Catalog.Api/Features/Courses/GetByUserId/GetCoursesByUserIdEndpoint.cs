using Microservice.Catalog.Api.Features.Courses.Dtos;
using Microservices.Shared.Filter;

namespace Microservice.Catalog.Api.Features.Courses.GetByUserId
{
    public record GetCoursesByUserIdQuery(Guid UserId) : IrequestByServiceResult<List<CourseDto>>;

    public class GetCoursesByUserIdHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetCoursesByUserIdQuery, ServiceResult<List<CourseDto>>>
    {
        public async Task<ServiceResult<List<CourseDto>>> Handle(GetCoursesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var courses = await context.Courses
                .Where(c => c.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (!courses.Any())
            {
                return ServiceResult<List<CourseDto>>.Error("No courses found for this user", System.Net.HttpStatusCode.NotFound);
            }

            var categories = await context.Categories.ToListAsync(cancellationToken);
            foreach (var course in courses)
            {
                course.Category = categories.First(c => c.Id == course.CategoryId);
            }

            var courseDtos = mapper.Map<List<CourseDto>>(courses);
            return ServiceResult<List<CourseDto>>.SuccesAsOkay(courseDtos);
        }
    }

    public static class GetCoursesByUserIdEndpoint
    {
        public static RouteGroupBuilder GetCoursesByUserIdItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/user/{userId:guid}", async (Guid userId, IMediator mediator) =>
                (await mediator.Send(new GetCoursesByUserIdQuery(userId))).ToGenericResult())
                .WithName("GetCoursesByUserId")
                .MapToApiVersion(1, 0).RequireAuthorization(policyNames: "InstructorPolicy");

            return groupBuilder;
        }
    }
}
