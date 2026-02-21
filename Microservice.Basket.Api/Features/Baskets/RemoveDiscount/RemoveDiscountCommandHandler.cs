using MediatR;
using Microservices.Shared;
using System.Net;

namespace Microservice.Basket.Api.Features.Baskets.RemoveDiscount
{
    public class RemoveDiscountCommandHandler(BasketService basketService)
        : IRequestHandler<RemoveDiscountCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(RemoveDiscountCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketService.GetBasketFromCache(cancellationToken);

            if (basket is null)
            {
                return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);
            }

            basket.ClearDiscount();
            await basketService.SaveBasketToCache(basket, cancellationToken);

            return ServiceResult.SuccesAsNoContent();
        }
    }
}
