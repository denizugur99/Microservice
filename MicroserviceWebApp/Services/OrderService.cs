using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MicroserviceWebApp.Services
{
    public class OrderService(IOrderRefitService orderRefitService, ILogger<OrderService> logger)
    {
        public async Task<ServiceResult<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request)
        {
            var response = await orderRefitService.CreateOrderAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content!);
                logger.LogError("Error creating order: {Title} - {Detail}", problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<CreateOrderResponse>.Error(problemDetails!);
            }
            return ServiceResult<CreateOrderResponse>.SuccesAsOkay(response.Content!);
        }
    }
}