using Asp.Versioning.Builder;
using Microservice.Payment.Api.Feature.Get;
using Microservice.Payment.Api.Feature.Payments.Create;

namespace Microservice.Payment.Api.Feature.Payments
{
    public static class PaymentEndpointExt
    {
        public static void AddPaymentGroupEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var paymentGroup = app.MapGroup("/api/v{version:apiVersion}/payments")
                .WithTags("Payments")
                .WithApiVersionSet(apiVersionSet)
                .CreatePaymentGroupItemEndpoint()
                .GetAllPaymentsByUserIdItemEndpoint();
        }
    }
}
