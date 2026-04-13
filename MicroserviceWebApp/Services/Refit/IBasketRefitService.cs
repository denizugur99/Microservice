using Refit;

namespace MicroserviceWebApp.Services.Refit
{
    public interface IBasketRefitService
    {
        [Get("/api/v1/baskets/user")]
        Task<ApiResponse<BasketDto>> GetBasketAsync();

        [Post("/api/v1/baskets/item")]
        Task<ApiResponse<object>> AddItemToBasketAsync([Body] AddBasketItemRequest request);

        [Delete("/api/v1/baskets/item/{courseId}")]
        Task<ApiResponse<object>> DeleteItemFromBasketAsync(Guid courseId);

        [Put("/api/v1/baskets/discount")]
        Task<ApiResponse<object>> ApplyDiscountAsync([Body] ApplyDiscountRequest request);

        [Delete("/api/v1/baskets/discount")]
        Task<ApiResponse<object>> RemoveDiscountAsync();
    }

    // Request/Response Models
    public record BasketDto
    {
        public List<BasketItemDto> Items { get; init; } = new();
        public float? DiscountRate { get; set; }
        public string? Coupon { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? TotalPriceWithDiscount { get; set; }
    }

    public record BasketItemDto(Guid Id, string Name, string ImageUrl, decimal Price, decimal? PriceByApplyDiscountRate);

    public record AddBasketItemRequest(Guid CourseId, string CourseName, decimal CoursePrice, string? ImageUrl);

    public record ApplyDiscountRequest(string Coupon, float Rate);
}
