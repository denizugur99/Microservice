namespace Microservice.Order.Application.Contracts.Refit.PaymentService
{
    public record CreatePaymentResponse(bool Status, string? ErrorMessage,Guid? PaymentId);
   
}
