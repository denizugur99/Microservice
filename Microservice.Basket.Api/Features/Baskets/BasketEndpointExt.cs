using Asp.Versioning.Builder;
using Microservice.Basket.Api.Features.Baskets.AddBasketItem;
using Microservice.Basket.Api.Features.Baskets.DeleteBasketItem;
using Microservice.Basket.Api.Features.Baskets.GetBasket;

namespace Microservice.Basket.Api.Features.Baskets
{
    public static class BasketEndpointExt
    {
        public static void AddBasketGroupEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var categoryGroup = app.MapGroup("/api/v{version:apiVersion}/baskets").WithTags("Baskets").WithApiVersionSet(apiVersionSet)
                .GetBasketGroupEndpoint()
                .AddBasketGroupItemEndpoint()
                .DeleteBasketGroupItemEndpoint();

        }
    }
}
