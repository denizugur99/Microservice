using Microservice.Payment.Api.Repositories;

namespace Microservice.Payment.Api.Feature.Get
{
    public record GetAllPaymentsByUserIdResponse(Guid Id, string OrderCode, string Amount, DateTimeOffset Created, PaymentStatus Status);
    
}
