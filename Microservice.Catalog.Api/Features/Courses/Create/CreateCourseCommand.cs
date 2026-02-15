namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public record CreateCourseCommand(string name, string description, decimal price, string ImageUrl, Guid categoryId) : IrequestByServiceResult<Guid>;
    
}
