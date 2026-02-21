using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Data;
using Microservice.Basket.Api.Dto;
using Microservices.Shared;
using Microservices.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.AddBasketItem
{
    public class AddBasketItemCommandHandler(IDistributedCache cache,IIdentityService ıdentityService) : IRequestHandler<AddBasketItemCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            Guid userId=ıdentityService.GetUserId;
            var cacheKey=string.Format(BasketConst.BasketCacheKey,userId);
            var basket=await  cache.GetStringAsync(cacheKey,cancellationToken);
            Data.Basket? currentBasket;
            var newBasketItem = new BasketItem(request.CourseId, request.CourseName, request.ImageUrl, request.CoursePrice, null);
            if (string.IsNullOrEmpty(basket)) {
                currentBasket = new Data.Basket(userId, [newBasketItem]);
                await CreateCacheAsync(currentBasket, cacheKey, cancellationToken);
                return ServiceResult.SuccesAsNoContent();
            }
             currentBasket=JsonSerializer.Deserialize<Data.Basket>(basket);
                var existingBasketItem = currentBasket!.Items.FirstOrDefault(x => x.Id == request.CourseId);
                if(existingBasketItem is not null)
                {
                    currentBasket.Items.Remove(existingBasketItem);
                   
                }
            currentBasket.Items.Add(newBasketItem);
            await CreateCacheAsync(currentBasket, cacheKey, cancellationToken);


            return ServiceResult.SuccesAsNoContent();

        }
        private async Task CreateCacheAsync (Data.Basket basket,string cachekey,CancellationToken cancellationToken)
        {
            var Jsonbasket = JsonSerializer.Serialize(basket);
            await cache.SetStringAsync(cachekey, Jsonbasket, cancellationToken);
        }
    }
}
