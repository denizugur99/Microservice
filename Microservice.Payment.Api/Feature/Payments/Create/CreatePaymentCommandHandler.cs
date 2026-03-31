using MediatR;
using Microservice.Payment.Api.Repositories;
using Microservices.Shared;
using Microservices.Shared.Services;

namespace Microservice.Payment.Api.Feature.Payments.Create
{
    public class CreatePaymentCommandHandler(AppDbContext dbContext,IIdentityService identityService):IRequestHandler<CreatePaymentCommand, ServiceResult<CreatePaymentResponse>>
    {
        public async Task<ServiceResult<CreatePaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var (isSucces, errorMessage) = await ExternalPaymentProcessAsync(request.CardNumber, request.CardHolderName, request.Expiration, request.Cvv, request.Amount);
            if(!isSucces)
            {
                return ServiceResult<CreatePaymentResponse>.Error("Payment failed",errorMessage ?? "Payment processing failed.", System.Net.HttpStatusCode.BadRequest);
            }
            var userId = identityService.GetUserId;
            var newPayment = new Repositories.Payment(userId,request.OrderCode,request.Amount);
            newPayment.SetPaymentStatus(PaymentStatus.Succes);
            dbContext.Payments.Add(newPayment);
            await dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult<CreatePaymentResponse>.SuccesAsOkay(new CreatePaymentResponse(true,null,newPayment.Id));
        }

        private async Task<(bool isSucces,string? errorMessage)> ExternalPaymentProcessAsync(string cardNumber, string CardHolderName, string expiration, string cvv, decimal amount)
        {
            // Simulate external payment processing
            await Task.Delay(1000); // Simulate network delay
            return (true, null);// Assume payment is successful for this example
           // return (false, "Payment failed due to insufficient funds."); // Simulate a failure case
        }
    }
}
