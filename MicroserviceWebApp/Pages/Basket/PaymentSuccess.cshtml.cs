using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Basket
{
    [Authorize]
    public class PaymentSuccessModel(DiscountService discountService, ILogger<PaymentSuccessModel> logger) : PageModel
    {
        public Guid OrderId { get; set; }

        [TempData]
        public string? DiscountCode { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return RedirectToPage("/Basket/Basket");
            }

            OrderId = orderId;

            // Ödeme başarılı oldu, eğer discount kullanıldıysa sil
            if (!string.IsNullOrEmpty(DiscountCode))
            {
                logger.LogInformation("Deleting used discount code: {Code} after successful payment", DiscountCode);
                var deleteResult = await discountService.DeleteDiscountAsync(DiscountCode);

                if (deleteResult.IsFail)
                {
                    logger.LogWarning("Failed to delete discount code: {Code} - {Error}", DiscountCode);
                }
                else
                {
                    logger.LogInformation("Successfully deleted discount code: {Code}", DiscountCode);
                }
            }

            return Page();
        }
    }
}
