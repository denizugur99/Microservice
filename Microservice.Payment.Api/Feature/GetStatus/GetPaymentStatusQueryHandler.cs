using MediatR;
using Microservice.Payment.Api.Repositories;
using Microservices.Shared;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Payment.Api.Feature.GetStatus
{

    public record GetPaymentStatusRequest(string orderCode) : IrequestByServiceResult<GetPaymentStatusResponse>;
    public record GetPaymentStatusResponse(Guid? paymentId,bool IsPaid);
    public class GetPaymentStatusQueryHandler(AppDbContext appDbContext) : IRequestHandler<GetPaymentStatusRequest, ServiceResult<GetPaymentStatusResponse>>
    {
        public async Task<ServiceResult<GetPaymentStatusResponse>> Handle(GetPaymentStatusRequest request, CancellationToken cancellationToken)
        {
            var payment= await appDbContext.Payments.FirstOrDefaultAsync(x=>x.OrderCode==request.orderCode, cancellationToken);

            if (payment == null) return ServiceResult<GetPaymentStatusResponse>.SuccesAsOkay(new GetPaymentStatusResponse(null,false));

            return ServiceResult<GetPaymentStatusResponse>.SuccesAsOkay(new GetPaymentStatusResponse(payment.Id,payment.Status==PaymentStatus.Succes));
        }
    }
}
