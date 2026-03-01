using Asp.Versioning;
using Microservice.Discount.API.Repositories;
using Microservices.Shared.Services;
using System.Security.Principal;

namespace Microservice.Discount.API.Features.Discount.CreateDiscount
{
    public class CreateDisocuntCommandHandler(AppDbContext context,IIdentityService identityService) : IRequestHandler<CreateDiscountCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = new Discount
            {
                Id = NewId.NextSequentialGuid(),
                Code=request.Code,
                Created= DateTimeOffset.UtcNow,
                Expire=request.ExpireDate,
                UserId=identityService.GetUserId
            };
           await context.Discounts.AddAsync(discount, cancellationToken);
           await context.SaveChangesAsync(cancellationToken);
            return ServiceResult.SuccesAsNoContent();
        }
    }
}
