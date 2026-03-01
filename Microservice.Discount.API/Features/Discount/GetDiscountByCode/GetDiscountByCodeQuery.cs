namespace Microservice.Discount.API.Features.Discount.GetDiscountByCode
{
    public record GetDiscountByCodeQuery(string Code): IrequestByServiceResult<DiscountDto>;

}
