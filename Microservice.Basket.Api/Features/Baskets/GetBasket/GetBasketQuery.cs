using Microservice.Basket.Api.Dto;
using Microservices.Shared;

namespace Microservice.Basket.Api.Features.Baskets.GetBasket
{
    public record GetBasketQuery : IrequestByServiceResult<BasketDto>;
}
