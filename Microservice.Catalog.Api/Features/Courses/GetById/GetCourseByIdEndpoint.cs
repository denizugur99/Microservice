using Microservice.Catalog.Api.Features.Courses.Dtos;
using Microservice.Catalog.Api.Features.Courses.GetAll;

namespace Microservice.Catalog.Api.Features.Courses.GetById
{
    public record GetCourseByIdQuery(Guid Id) : IrequestByServiceResult<CourseDto>;

    public class GetCourseByIdQueryHandler(AppDbContext context,IMapper mapper) : IRequestHandler<GetCourseByIdQuery, ServiceResult<CourseDto>>
    {
        public async Task<ServiceResult<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var hascourse= await context.Courses.FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
            if (hascourse is null) { 
            return ServiceResult<CourseDto>.Error("Course Not Found", HttpStatusCode.NotFound);
            }
            var category = await context.Categories.FindAsync(hascourse.CategoryId, cancellationToken);
            hascourse.Category = category!;


            var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            var courseDto = mapper.Map<CourseDto>(course);
            return ServiceResult<CourseDto>.SuccesAsOkay(courseDto);
           
        }
    }

    public static class GetCourseByIdEndpoint
    {
        public static RouteGroupBuilder GetCourseByIdItemEndpoint(this RouteGroupBuilder groupBuilder)
        {
            groupBuilder.MapGet("/{id:guid}", async (IMediator mediator,Guid id) => (await mediator.Send(new GetCourseByIdQuery(id))).ToGenericResult()).WithName("GetByIdCourse")
                .MapToApiVersion(1, 0);

            return groupBuilder;
        }
    }
}
