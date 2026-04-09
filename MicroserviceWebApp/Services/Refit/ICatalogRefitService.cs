using MicroserviceWebApp.Pages.Instructor.Dto;
using Refit;

namespace MicroserviceWebApp.Services.Refit
{
    public interface ICatalogRefitService
    {
        [Post("/api/v1/courses")]
        [Multipart]
        Task<Guid> CreateCourseAsync([Body] CreateCourseRequest request);

        [Put("/api/v1/courses")]
        Task UpdateCourseAsync([Body] UpdateCourseRequest request);

        [Delete("/api/v1/courses/{id}")]
        Task DeleteCourseAsync(Guid id);
    }
}
