namespace MicroserviceWebApp.Pages.Instructor.Dto
{
    public record CourseDto(Guid Id, string Name, string Description,string ImageUrl, decimal Price,CategoryDto Category, FeatureDto Feature);
    
}