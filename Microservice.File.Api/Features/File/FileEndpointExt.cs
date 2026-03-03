using Asp.Versioning.Builder;
using Microservice.Discount.API.Features.Discount.CreateDiscount;
using Microservice.Discount.API.Features.Discount.DeleteDiscount;


namespace Microservice.Discount.API.Features.Discount
{
    public static class FileEndpointExt
    {
        public static void AddFileGroupEndpoints(this WebApplication app,ApiVersionSet apiVersionSet)
        {
            var categoryGroup = app.MapGroup("/api/v{version:apiVersion}/files").WithTags("Files").WithApiVersionSet(apiVersionSet)
                .UploadFileEndpoint()
                .DeleteFileEndpoint();



        }
    }
}
