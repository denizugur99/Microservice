using Microservice.Catalog.Api.Features.Courses;
using Microservice.Catalog.Api.Repositories;

namespace Microservice.Catalog.Api.Features.Categories
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }=default!;
        public List<Course>? Courses { get; set; }
    }
}
