using MicroserviceWebApp.Pages.Instructor.Dto;
using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services.Refit;
using Refit;
using System.Text.Json;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace MicroserviceWebApp.Services
{
    public class CatalogService(ICatalogRefitService catalogRefitService, ILogger<CatalogService> logger)
    {
        public async Task<ServiceResult<List<CategoryViewModel>>> GetCategoriesAsync()
        {
            var response = await catalogRefitService.GetCategoriesAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);

                logger.LogError("Error fetching categories: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<CategoryViewModel>>.Error(problemDetails!);
            }

            var categories = response.Content!.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return ServiceResult<List<CategoryViewModel>>.SuccesAsOkay(categories);
        }

        public async Task<ServiceResult> CreateCourseAsync(CreateCourseViewModel model)
        {
            StreamPart? pictureStreamPart = null;
            await using var stream = model.Picture?.OpenReadStream();
            if (model.Picture is not null && model.Picture.Length > 0)
            {
                 
                pictureStreamPart = new StreamPart(stream!, model.Picture!.FileName, model.Picture.ContentType);
            }

            var response = await catalogRefitService.CreateCourseAsync(
            name: model.Name,
            description: model.Description,
            price: model.Price,
            picture: pictureStreamPart,
            categoryId: model.CategoryId.ToString()
            );

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error creating course: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult.Error(problemDetails!);
            }

            return ServiceResult.Success();
        }
    }
}
