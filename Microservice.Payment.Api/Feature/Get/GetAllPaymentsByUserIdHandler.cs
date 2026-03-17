using MediatR;
using Microservice.Payment.Api.Repositories;
using Microservices.Shared;
using Microservices.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Payment.Api.Feature.Get
{
    public class GetAllPaymentsByUserIdHandler(AppDbContext context,IIdentityService identityService) : IRequestHandler<GetAllPaymentsByUserIdQuery, ServiceResult<List<GetAllPaymentsByUserIdResponse>>>
    {
        public async Task<ServiceResult<List<GetAllPaymentsByUserIdResponse>>> Handle(GetAllPaymentsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userId = identityService.GetUserId;
            var userName = identityService.GetUserName;
            var userRoles = identityService.Roles;
            var payments = await context.Payments.Where(p => p.UserId == userId).Select(p => new GetAllPaymentsByUserIdResponse(

             p.Id,
             p.OrderCode,
             p.Amount.ToString("C"),
             p.Created,
             p.Status
           )).ToListAsync(cancellationToken);
            return ServiceResult<List<GetAllPaymentsByUserIdResponse>>.SuccesAsOkay(payments);
        }
    }
}
