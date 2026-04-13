using Refit;

namespace MicroserviceWebApp.Services.Refit
{
    public interface IOrderRefitService
    {
        [Post("/api/v1.0/orders")]
        Task<ApiResponse<CreateOrderResponse>> CreateOrderAsync([Body] CreateOrderRequest request);

        [Get("/api/v1.0/orders")]
        Task<ApiResponse<List<OrderDto>>> GetOrdersAsync();
    }

    // Request/Response Models
    public record CreateOrderRequest(
        decimal Discount,
        AddressDto Address,
        PaymentDto Payment,
        List<OrderItemDto> Orders
    );

    public record CreateOrderResponse(Guid OrderId, string PaymentStatus);

    public record OrderDto(
        Guid Id,
        string UserId,
        decimal Discount,
        AddressDto Address,
        List<OrderItemDto> OrderItems,
        decimal TotalPrice,
        DateTime CreatedDate,
        string Status
    );

    public record AddressDto(
        string Province,
        string City,
        string Region,
        string Street,
        string PostalCode
    );

    public record PaymentDto(
        string CardName,
        string CardNumber,
        string Expiration,
        string Cvv,
        decimal Amount
    );

    public record OrderItemDto(
        Guid ProductId,
        string ProductName,
        decimal UnitPrice
    );
}
