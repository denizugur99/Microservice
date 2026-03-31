using Microservices.Shared;

namespace Microservice.Order.Application.Contracts.Refit.PaymentService
{
    public record CreatePaymentRequest(string OrderCode, string CardNumber, string CardHolderName, string Expiration, string Cvv, decimal Amount);



}
