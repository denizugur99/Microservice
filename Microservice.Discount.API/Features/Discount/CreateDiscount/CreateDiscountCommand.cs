namespace Microservice.Discount.API.Features.Discount.CreateDiscount
{
    public record CreateDiscountCommand(string Code, float Rate, Guid UserId,DateTimeOffset ExpireDate) : IrequestByServiceResult;
   
}
