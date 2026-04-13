using MicroserviceWebApp.Pages.Basket.ViewModel;
using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceWebApp.Services
{
    public class BasketService(IBasketRefitService basketRefitService, ILogger<BasketService> logger, UserService userService)
    {
        public async Task<ServiceResult> AddItemToBasketAsync(AddBasketItemRequest request)
        {
            var response = await basketRefitService.AddItemToBasketAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error adding item to basket: {StatusCode} - {Content}", response.StatusCode, response.Error.Content);
                return ServiceResult.Error(problemDetails!);
            }
            return ServiceResult.Success();
        }
        public async Task<ServiceResult<BasketViewModel>> GetBasket()
        {
            var response = await basketRefitService.GetBasketAsync();
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error retrieving basket: {StatusCode} - {Content}", response.StatusCode, response.Error.Content);
                return ServiceResult<BasketViewModel>.Error(problemDetails!);
            }
            var basket = new BasketViewModel()
            {
                UserId = string.Empty, // Backend'de UserId yok
                Items = response.Content.Items.Select(i => new BasketItemViewModel
                {
                    CoursePrice = i.Price,
                    CourseName = i.Name,
                    CourseId = i.Id,
                    ImageUrl = i.ImageUrl,
                }).ToList(),
                Discount = response.Content.DiscountRate.HasValue && !string.IsNullOrEmpty(response.Content.Coupon)
                    ? new BasketDiscountViewModel
                    {
                        Coupon = response.Content.Coupon,
                        Rate = (decimal)response.Content.DiscountRate.Value
                    }
                    : null
            };

            return ServiceResult<BasketViewModel>.SuccesAsOkay(basket);



        }
        public async Task<ServiceResult> DeleteItemFromBasketAsync(Guid courseId)
        {
            var response = await basketRefitService.DeleteItemFromBasketAsync(courseId);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error deleting item from basket: {StatusCode} - {Content}", response.StatusCode, response.Error.Content);
                return ServiceResult.Error(problemDetails!);
            }
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> ApplyDiscountAsync(string coupon, float rate)
        {
            var request = new ApplyDiscountRequest(coupon, rate);
            var response = await basketRefitService.ApplyDiscountAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error applying discount: {StatusCode} - {Content}", response.StatusCode, response.Error.Content);
                return ServiceResult.Error(problemDetails!);
            }
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> RemoveDiscountAsync()
        {
            var response = await basketRefitService.RemoveDiscountAsync();
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error removing discount: {StatusCode} - {Content}", response.StatusCode, response.Error.Content);
                return ServiceResult.Error(problemDetails!);
            }
            return ServiceResult.Success();
        }
    }
}