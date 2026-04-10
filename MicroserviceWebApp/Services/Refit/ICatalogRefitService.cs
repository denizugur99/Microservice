using MicroserviceWebApp.Pages.Instructor.Dto;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace MicroserviceWebApp.Services.Refit
{
    public interface ICatalogRefitService
    {
        [Get("/api/v1.0/categories")]
        Task<ApiResponse<List<CategoryDto>>> GetCategoriesAsync();

        [Multipart]
        [Post("/api/v1.0/course")]
        Task<ApiResponse<object>> CreateCourseAsync(
            [AliasAs("Name")] string name,
            [AliasAs("Description")] string description,
            [AliasAs("Price")] decimal price,
            [AliasAs("Picture")] StreamPart? picture,
            [AliasAs("CategoryId")] string categoryId);
         

        [Put("/api/v1.0/course")]
        Task<ApiResponse<object>> UpdateCourseAsync( UpdateCourseRequest request);

        [Delete("/api/v1.0/course/{id}")]
        Task<ApiResponse<object>> DeleteCourseAsync(Guid id);
    }
}
