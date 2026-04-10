namespace Microservice.Catalog.Api.Features.Courses.Create
{
    public record CreateCourseCommand: IrequestByServiceResult<Guid>
    {
        public string name { get; init; }=null!;
        public string description { get; init; }=null!;
        public decimal price { get; init; }
        public IFormFile? picture { get; init; }
        public Guid categoryId { get; init; }   
    };
    
}
