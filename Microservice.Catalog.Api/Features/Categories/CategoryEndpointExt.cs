using Microservice.Catalog.Api.Features.Categories.Create;
using Microservice.Catalog.Api.Features.Categories.GetAll;
using Microservice.Catalog.Api.Features.Categories.GetById;

namespace Microservice.Catalog.Api.Features.Categories
{
    public static class CategoryEndpointExt
    {
        public static void AddCategoryGroupEndpoints(this WebApplication app)
        {
            var categoryGroup = app.MapGroup("/api/categories").WithTags("Categories")
                .CreateCategoryGroupItemEndpoint()
                .ListCategoryGroupItemEndpoint()
                .GetByIdCategoryGroupItemEndpoint();

        }
    }
}
