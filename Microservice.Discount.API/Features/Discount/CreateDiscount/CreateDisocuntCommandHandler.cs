using Asp.Versioning;
using Microservice.Bus.Events;
using Microservice.Discount.API.Repositories;
using Microservices.Shared.Services;
using System.Security.Principal;

namespace Microservice.Discount.API.Features.Discount.CreateDiscount
{
    public class CreateDisocuntCommandHandler(AppDbContext context,ITopicProducer<DiscountNotificationEvent> topicProducer) : IRequestHandler<CreateDiscountCommand, ServiceResult>
    {
        
        public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
           

            var hasCode=await context.Discounts.AnyAsync(x => x.Code == request.Code && x.UserId == request.UserId, cancellationToken);
            if (hasCode)
            {
                return ServiceResult.Error("Code already exists for this user", HttpStatusCode.BadRequest);
            }
            
            var discount = new Discount
            {
                Id = NewId.NextSequentialGuid(),
                Code=request.Code,
                Created= DateTimeOffset.UtcNow,
                Expire=request.ExpireDate,
                UserId=request.UserId,
                Rate=request.Rate

            };
            
           await context.Discounts.AddAsync(discount, cancellationToken);
           await context.SaveChangesAsync(cancellationToken);
        

            return ServiceResult.SuccesAsNoContent();
        }
    }
}
