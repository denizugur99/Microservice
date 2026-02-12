using Microservice.Catalog.Api.Features.Categories.Create;
using Microservices.Shared.Filter;

namespace Microservice.Catalog.Api.Features.Categories
{
    public static class CategoryEndpointExt
    {
        public static void AddCategoryGroupEndpoints(this WebApplication app)
        {
            var categoryGroup = app.MapGroup("app/categories").CreateCategoryGroupItemEndpoint();

        }
    }
}
