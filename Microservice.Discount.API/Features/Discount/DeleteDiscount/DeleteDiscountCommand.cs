namespace Microservice.Discount.API.Features.Discount.DeleteDiscount
{
    public record DeleteDiscountCommand(string Code) : IrequestByServiceResult;
}
