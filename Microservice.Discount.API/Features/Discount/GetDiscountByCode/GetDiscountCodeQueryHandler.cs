using Microservice.Discount.API.Repositories;
using Microservices.Shared.Services;

namespace Microservice.Discount.API.Features.Discount.GetDiscountByCode
{
    public class GetDiscountCodeQueryHandler(AppDbContext context) : IRequestHandler<GetDiscountByCodeQuery, ServiceResult<DiscountDto>>
    {
        public async Task<ServiceResult<DiscountDto>> Handle(GetDiscountByCodeQuery request, CancellationToken cancellationToken)
        {
            var hasDiscount=await context.Discounts.SingleOrDefaultAsync(x=>x.Code==request.Code);

            if(hasDiscount is null)
            {
                return  ServiceResult<DiscountDto>.Error("Not Found", $"There is no discount with code {request.Code}", HttpStatusCode.NotFound);
            }
            if(hasDiscount.Expire<DateTimeOffset.Now)
            {
                return ServiceResult<DiscountDto>.Error("Bad Request", $"The discount with code {request.Code} is expired", HttpStatusCode.BadRequest);
            }
            return ServiceResult<DiscountDto>.SuccesAsOkay(new DiscountDto(hasDiscount.Rate,hasDiscount.Code));
        }
    }
}
