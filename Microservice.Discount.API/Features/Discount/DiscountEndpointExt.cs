using Asp.Versioning.Builder;
using Microservice.Discount.API.Features.Discount.CreateDiscount;
using Microservice.Discount.API.Features.Discount.DeleteDiscount;
using Microservice.Discount.API.Features.Discount.GetDiscountByCode;


namespace Microservice.Discount.API.Features.Discount
{
    public static class DiscountEndpointExt
    {
        public static void AddDiscountGroupEndpoints(this WebApplication app,ApiVersionSet apiVersionSet)
        {
            var categoryGroup = app.MapGroup("/api/v{version:apiVersion}/discount").WithTags("Discount").WithApiVersionSet(apiVersionSet)
                .CreateDiscountEndpoint()
                .GetDiscountEndpoint()
                .DeleteDiscountItemEndpoint()
                .RequireAuthorization("ClientCredential");


        }
    }
}
