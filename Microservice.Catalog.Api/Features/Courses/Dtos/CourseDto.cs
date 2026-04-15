namespace Microservice.Catalog.Api.Features.Courses.Dtos
{
    public record CourseDto(Guid Id, string Name, string Description, string ImageUrl, decimal Price, Guid UserId, CategoryDto Category, FeatureDto Feature);

}