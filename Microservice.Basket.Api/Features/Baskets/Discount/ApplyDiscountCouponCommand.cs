using Microservices.Shared;

namespace Microservice.Basket.Api.Features.Baskets.Discount
{
    public record ApplyDiscountCouponCommand(string Coupon, float Rate) : IrequestByServiceResult;
    
}
