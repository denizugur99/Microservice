using AutoMapper;
using MediatR;
using Microservice.Basket.Api.Const;
using Microservice.Basket.Api.Dto;
using Microservices.Shared;
using Microservices.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket
{
    public class GetBasketQueryHandler(IDistributedCache cache, IIdentityService identityService,IMapper mapper)
        : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
    {
        public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            Guid userId = identityService.GetUserId;
            var cacheKey = string.Format(BasketConst.BasketCacheKey, userId);
            var basket = await cache.GetStringAsync(cacheKey, cancellationToken);
           
            if (string.IsNullOrEmpty(basket))
            {
                return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);
            }

            var currentBasket = JsonSerializer.Deserialize<Data.Basket>(basket);
            var basketDto = mapper.Map<BasketDto>(currentBasket);

            if (currentBasket is null)
            {
                return ServiceResult<BasketDto>.Error("Basket deserialization failed", HttpStatusCode.InternalServerError);
            }

            return ServiceResult<BasketDto>.SuccesAsOkay(basketDto);
        }
    }
}
