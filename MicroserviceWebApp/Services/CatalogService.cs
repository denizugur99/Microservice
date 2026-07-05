using MicroserviceWebApp.Pages.Instructor.Dto;
using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services.Refit;
using Refit;
using System.Text.Json;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace MicroserviceWebApp.Services
{
    public class CatalogService(ICatalogRefitService catalogRefitService, ILogger<CatalogService> logger,UserService userService)
    {
        public async Task<ServiceResult<List<CategoryViewModel>>> GetCategoriesAsync()
        {
            var response = await catalogRefitService.GetCategoriesAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);

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
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);
                logger.LogError("Error creating course: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult.Error(problemDetails!);
            }

            return ServiceResult.Success();
        }



        public async Task<ServiceResult<List<CourseViewModel>>> GetAllCoursesAsync()
        {
            var response = await catalogRefitService.GetAllCoursesAsync();
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);
                logger.LogError("Error fetching all courses: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<CourseViewModel>>.Error(problemDetails!);
            }
            var courses = response.Content!.Select(c => new CourseViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                CategoryId = c.Category.Id,
                CategoryName = c.Category.Name,
                Duration = c.Feature.Duration,
                Rating = c.Feature.Rating,
                UserName = c.Feature.EducatorFullName,
                UserId = c.UserId
            }).ToList();
            return ServiceResult<List<CourseViewModel>>.SuccesAsOkay(courses);
        }

        public async Task<ServiceResult<List<CourseViewModel>>> GetCoursesAsync()
        {
            var response = await catalogRefitService.GetCoursesByUserIdAsync(userService.GetUserId);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);
                logger.LogError("Error fetching courses: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<CourseViewModel>>.Error(problemDetails!);
            }
            var courses = response.Content!.Select(c => new CourseViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                CategoryId = c.Category.Id,
                CategoryName = c.Category.Name,
                Duration = c.Feature.Duration,
                Rating = c.Feature.Rating,
                UserName = c.Feature.EducatorFullName,
                UserId = c.UserId
            }).ToList();
            return ServiceResult<List<CourseViewModel>>.SuccesAsOkay(courses);
        }

        public async Task<ServiceResult> DeleteCourseAsync(Guid courseId)
        {
            var response = await catalogRefitService.DeleteCourseAsync(courseId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);
                logger.LogError("Error deleting course: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult.Error(problemDetails!);
            }

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<CourseViewModel>> GetCourseByIdAsync(Guid courseId)
        {
            var response = await catalogRefitService.GetCourseByIdAsync(courseId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);
                logger.LogError("Error fetching course: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<CourseViewModel>.Error(problemDetails!);
            }

            var course = response.Content!;
            var courseViewModel = new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                Price = course.Price,
                CategoryId = course.Category.Id,
                CategoryName = course.Category.Name,
                Duration = course.Feature.Duration,
                Rating = course.Feature.Rating,
                UserName = course.Feature.EducatorFullName,
                UserId = course.UserId
            };

            return ServiceResult<CourseViewModel>.SuccesAsOkay(courseViewModel);
        }

        public async Task<ServiceResult> UpdateCourseAsync(UpdateCourseRequest request)
        {
            var response = await catalogRefitService.UpdateCourseAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(((global::Refit.ApiException)response.Error!).Content!);
                logger.LogError("Error updating course: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult.Error(problemDetails!);
            }

            return ServiceResult.Success();
        }
    }
}
