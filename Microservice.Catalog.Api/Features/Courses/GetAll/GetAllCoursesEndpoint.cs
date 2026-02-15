using Microservice.Catalog.Api.Features.Courses.Create;
using Microservice.Catalog.Api.Features.Courses.Dtos;
using Microservices.Shared.Filter;

namespace Microservice.Catalog.Api.Features.Courses.GetAll
{

    public class GetAllCoursesQuery() : IrequestByServiceResult<List<CourseDto>>;

    public class GetAllCoursesHandler(AppDbContext context, IMapper mapper) : IRequestHandler<GetAllCoursesQuery, ServiceResult<List<CourseDto>>>
    {
        public async Task<ServiceResult<List<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
           var courses=await context.Courses.ToListAsync(cancellationToken);
            var categories=await context.Categories.ToListAsync(cancellationToken);
            foreach (var course in courses)
            {
                course.Category= categories.First(c=>c.Id==course.CategoryId);
            }
            var courseDtos=mapper.Map<List<CourseDto>>(courses);
            return ServiceResult<List<CourseDto>>.SuccesAsOkay(courseDtos);
        }
    }

    public static class GetAllCoursesEndpoint
    {
        public static RouteGroupBuilder GetAllCourseEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/", async ( IMediator mediator) => (await mediator.Send(new GetAllCoursesQuery())).ToGenericResult()).WithName("GetAllCourse");
               
            return groupBuilder;
        }

    }
}
