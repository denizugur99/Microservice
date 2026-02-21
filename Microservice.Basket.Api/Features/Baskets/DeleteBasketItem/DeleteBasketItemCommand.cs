using Microservices.Shared;

namespace Microservice.Basket.Api.Features.Baskets.DeleteBasketItem
{
    public record DeleteBasketItemCommand(Guid Id) : IrequestByServiceResult;
    
}
