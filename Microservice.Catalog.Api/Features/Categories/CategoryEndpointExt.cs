using Asp.Versioning.Builder;
using Microservice.Catalog.Api.Features.Categories.Create;
using Microservice.Catalog.Api.Features.Categories.GetAll;
using Microservice.Catalog.Api.Features.Categories.GetById;

namespace Microservice.Catalog.Api.Features.Categories
{
    public static class CategoryEndpointExt
    {
        public static void AddCategoryGroupEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var categoryGroup = app.MapGroup("/api/v{version:apiVersion}/categories").WithTags("Categories").WithApiVersionSet(apiVersionSet)
                .CreateCategoryGroupItemEndpoint()
                .ListCategoryGroupItemEndpoint()
                .GetByIdCategoryGroupItemEndpoint().RequireAuthorization("ClientCredential");

        }
    }
}
