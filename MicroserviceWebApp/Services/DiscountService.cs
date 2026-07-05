using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceWebApp.Services
{
    public class DiscountService(IDiscountRefitService discountRefitService, ILogger<DiscountService> logger)
    {
        public async Task<ServiceResult<DiscountDto>> GetDiscountByCodeAsync(string code)
        {
            try
            {
                var response = await discountRefitService.GetDiscountByCodeAsync(code);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Error getting discount by code: {0} - {1}", code, response.StatusCode);

                    // Eğer error content varsa parse et
                    if (!string.IsNullOrEmpty(response.Error?.Content))
                    {
                        try
                        {
                            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content);
                            if (problemDetails != null)
                            {
                                return ServiceResult<DiscountDto>.Error(problemDetails);
                            }
                        }
                        catch (JsonException)
                        {
                            // JSON parse edilemezse genel hata döndür
                            logger.LogWarning("Could not parse error response as JSON for discount code: {Code}", code);
                        }
                    }

                    return ServiceResult<DiscountDto>.Error("Discount not found or expired");
                }

                return ServiceResult<DiscountDto>.SuccesAsOkay(response.Content!);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while getting discount by code: {Code}", code);
                return ServiceResult<DiscountDto>.Error("An error occurred while retrieving the discount");
            }
        }

        public async Task<ServiceResult> DeleteDiscountAsync(string code)
        {
            try
            {
                var response = await discountRefitService.DeleteDiscountAsync(code);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Error deleting discount: {0} - {1}", code, response.StatusCode);
                    return ServiceResult.Error("Failed to delete discount");
                }

                logger.LogInformation("Discount deleted successfully: {Code}", code);
                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while deleting discount: {Code}", code);
                return ServiceResult.Error("An error occurred while deleting the discount");
            }
        }
    }
}
