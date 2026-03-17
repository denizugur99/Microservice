using Asp.Versioning.Builder;
using Microservice.Catalog.Api.Features.Categories.Create;
using Microservice.Catalog.Api.Features.Courses.Create;
using Microservice.Catalog.Api.Features.Courses.Delete;
using Microservice.Catalog.Api.Features.Courses.GetAll;
using Microservice.Catalog.Api.Features.Courses.GetById;
using Microservice.Catalog.Api.Features.Courses.GetByUserId;
using Microservice.Catalog.Api.Features.Courses.Update;

namespace Microservice.Catalog.Api.Features.Courses
{
    public static class CourseEndpointExt
    {
        public static void AddCourseGroupEndpoints(this WebApplication app,ApiVersionSet apiVersionSet)
        {
            var categoryGroup = app.MapGroup("/api/v{version:apiVersion}/course").WithTags("Courses").WithApiVersionSet(apiVersionSet)
                .CreateCourseEndpoint()
                .GetAllCourseEndpoint()
                .GetCourseByIdItemEndpoint()
                .GetCoursesByUserIdItemEndpoint()
                .UpdateCourseEndpoint()
                .DeleteCourseItemEndpoint().RequireAuthorization("ClientCredential");

        }
    }
}
