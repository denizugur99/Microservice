using MediatR;
using Microservice.Basket.Api.Data;
using Microservices.Shared;
using Microservices.Shared.Services;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem
{
    public class AddBasketItemCommandHandler(BasketService basketService, IIdentityService identityService)
        : IRequestHandler<AddBasketItemCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            var currentBasket = await basketService.GetBasketFromCache(cancellationToken);
            var newBasketItem = new BasketItem(request.CourseId, request.CourseName, request.ImageUrl, request.CoursePrice, null);

            if (currentBasket is null)
            {
                currentBasket = new Data.Basket(identityService.GetUserId, [newBasketItem]);
                await basketService.SaveBasketToCache(currentBasket, cancellationToken);
                return ServiceResult.SuccesAsNoContent();
            }

            var existingBasketItem = currentBasket.Items.FirstOrDefault(x => x.Id == request.CourseId);
            if (existingBasketItem is not null)
            {
                currentBasket.Items.Remove(existingBasketItem);
            }

            currentBasket.Items.Add(newBasketItem);
            currentBasket.ApplyAvailableDiscount();

            await basketService.SaveBasketToCache(currentBasket, cancellationToken);

            return ServiceResult.SuccesAsNoContent();
        }
    }
}
