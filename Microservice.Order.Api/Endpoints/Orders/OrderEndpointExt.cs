using Asp.Versioning.Builder;


namespace Microservice.Order.Api.Endpoints.Orders
{ 
    public static class OrderEndpointExt
    {
        public static void AddOrderGroupEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
        var categoryGroup = app.MapGroup("/api/v{version:apiVersion}/orders").WithTags("Orders").WithApiVersionSet(apiVersionSet)
        .CreateOrderGroupItemEndpoint();
                

        }
    }
}
