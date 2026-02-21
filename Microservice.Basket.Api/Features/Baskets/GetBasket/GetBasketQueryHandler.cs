using AutoMapper;
using MediatR;
using Microservice.Basket.Api.Dto;
using Microservices.Shared;
using System.Net;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket
{
    public class GetBasketQueryHandler(BasketService basketService, IMapper mapper)
        : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
    {
        public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var currentBasket = await basketService.GetBasketFromCache(cancellationToken);

            if (currentBasket is null)
            {
                return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);
            }

            var basketDto = mapper.Map<BasketDto>(currentBasket);

            return ServiceResult<BasketDto>.SuccesAsOkay(basketDto);
        }
    }
}
