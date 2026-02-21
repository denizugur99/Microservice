using MediatR;
using Microservices.Shared;
using System.Net;

namespace Microservice.Basket.Api.Features.Baskets.Discount
{
    public class ApplyDiscouıntCommandHandler(BasketService basketService)
        : IRequestHandler<ApplyDiscountCouponCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketService.GetBasketFromCache(cancellationToken);

            if (basket is null)
            {
                return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);
            }

            if (!basket.Items.Any())
            {
                return ServiceResult.Error("Basket item not found", HttpStatusCode.NotFound);
            }

            basket.ApplyNewDiscount(request.Coupon, request.Rate);
            await basketService.SaveBasketToCache(basket, cancellationToken);

            return ServiceResult.SuccesAsNoContent();
        }
    }
}
