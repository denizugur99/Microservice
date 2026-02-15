namespace Microservice.Catalog.Api.Features.Categories
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping() { 
            CreateMap<Category, Dtos.CategoryDto>().ReverseMap();
        }
    }
}
