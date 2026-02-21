using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Dto;
using Microservices.Shared;
using Microservices.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Security.Principal;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem
{
    public class DeleteBasketItemCommandHandler(IDistributedCache distributedCache,IIdentityService ıdentityService) : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            Guid userId = ıdentityService.GetUserId;
            var cacheKey = string.Format(BasketConst.BasketCacheKey, userId);
            var basket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);
            if (string.IsNullOrEmpty(basket))
            {
                return ServiceResult.Error("basket not found",HttpStatusCode.NotFound);
            }
            var currentBasket = JsonSerializer.Deserialize<Data.Basket>(basket);
            var basketItemDelete=currentBasket!.Items.FirstOrDefault(x=>x.Id==request.Id);
            if(basketItemDelete is null)
            {
                return ServiceResult.Error("basket item not found", HttpStatusCode.NotFound);
            }
            currentBasket.Items.Remove(basketItemDelete);
            var jsonBasket = JsonSerializer.Serialize(currentBasket);
            await distributedCache.SetStringAsync(cacheKey, jsonBasket, cancellationToken);
            return ServiceResult.SuccesAsNoContent();
        }
    }
}
