using Microservice.Basket.Api.Const;
using Microservices.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets
{
    public class BasketService(IIdentityService ıdentityService, IDistributedCache distributedCache)
    {
        private string GetCacheKey()
        {
            return string.Format(BasketConst.BasketCacheKey, ıdentityService.GetUserId);
        }
        private string GetCacheKey(Guid userId)
        {
            return string.Format(BasketConst.BasketCacheKey, userId);
        }

        public async Task<string?> GetBasketJsonFromCache(CancellationToken cancellationToken)
        {
            return await distributedCache.GetStringAsync(GetCacheKey(), cancellationToken);
        }

        public async Task<Data.Basket?> GetBasketFromCache(CancellationToken cancellationToken)
        {
            var basketJson = await GetBasketJsonFromCache(cancellationToken);
            if (string.IsNullOrEmpty(basketJson))
            {
                return null;
            }
            return JsonSerializer.Deserialize<Data.Basket>(basketJson);
        }

        public async Task SaveBasketToCache(Data.Basket basket, CancellationToken cancellationToken)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            await distributedCache.SetStringAsync(GetCacheKey(), jsonBasket, cancellationToken);
        }
        public async Task DeleteBasketAsync(Guid userId)
        {
            await distributedCache.RemoveAsync(GetCacheKey(userId));
        }
    }
}