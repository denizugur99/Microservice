using Microservices.Shared;

namespace Microservice.Payment.Api.Feature.Get
{
    public record GetAllPaymentsByUserIdQuery : IrequestByServiceResult<List<GetAllPaymentsByUserIdResponse>>;
    
}
