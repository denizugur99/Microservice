using Microservices.Shared;

namespace Microservice.Payment.Api.Feature.Payments.Create
{
    public record CreatePaymentCommand(string OrderCode, string CardNumber,string CardHolderName ,string Expiration, string Cvv, decimal Amount) : IrequestByServiceResult<Guid>;
    
}
