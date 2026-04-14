using Refit;

namespace MicroserviceWebApp.Services.Refit
{
    public interface IDiscountRefitService
    {
        [Get("/api/v1/discount/{code}")]
        Task<ApiResponse<DiscountDto>> GetDiscountByCodeAsync(string code);

        [Delete("/api/v1/discount/{code}")]
        Task<ApiResponse<object>> DeleteDiscountAsync(string code);
    }

    // Response Model
    public record DiscountDto(float Rate, string Code);
}
