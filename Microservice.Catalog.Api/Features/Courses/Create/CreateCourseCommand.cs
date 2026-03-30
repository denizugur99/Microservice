namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public record CreateCourseCommand(string name, string description, decimal price, IFormFile? Picture, Guid categoryId) : IrequestByServiceResult<Guid>;
    
}
