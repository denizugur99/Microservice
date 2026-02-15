using Microservice.Catalog.Api.Features.Categories.Create;
using Microservice.Catalog.Api.Features.Courses.Create;
using Microservice.Catalog.Api.Features.Courses.GetAll;
using Microservice.Catalog.Api.Features.Courses.GetById;

namespace Microservice.Catalog.Api.Features.Courses
{
    public static class CourseEndpointExt
    {
        public static void AddCourseGroupEndpoints(this WebApplication app)
        {
            var categoryGroup = app.MapGroup("/api/course").WithTags("Courses")
                .CreateCourseEndpoint()
                .GetAllCourseEndpoint()
                .GetCourseByIdItemEndpoint();

        }
    }
}
