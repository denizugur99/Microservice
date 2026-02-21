using MediatR;
using Microservices.Shared;
using System.Net;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem
{
    public class DeleteBasketItemCommandHandler(BasketService basketService)
        : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            var currentBasket = await basketService.GetBasketFromCache(cancellationToken);

            if (currentBasket is null)
            {
                return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);
            }

            var basketItemToDelete = currentBasket.Items.FirstOrDefault(x => x.Id == request.Id);
            if (basketItemToDelete is null)
            {
                return ServiceResult.Error("Basket item not found", HttpStatusCode.NotFound);
            }

            currentBasket.Items.Remove(basketItemToDelete);
            await basketService.SaveBasketToCache(currentBasket, cancellationToken);

            return ServiceResult.SuccesAsNoContent();
        }
    }
}
