using MicroserviceWebApp.Pages.Basket.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Basket
{
    [Authorize]
    public class BasketModel(BasketService basketService, DiscountService discountService) : PageModel
    {
        public BasketViewModel? Basket { get; set; }

        [TempData]
        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            var result = await basketService.GetBasket();
            if (result.IsFail)
            {
                Basket = new BasketViewModel(); // Empty basket
                Message = "Sepetiniz şu anda boş.";
            }
            else
            {
                Basket = result.Data;
            }
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(Guid courseId)
        {
            var result = await basketService.DeleteItemFromBasketAsync(courseId);
            if (result.IsFail)
            {
                Message = "Ürün sepetten kaldırılırken bir hata oluştu.";
            }
            else
            {
                Message = "Ürün başarıyla sepetten kaldırıldı.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApplyDiscountAsync(string coupon)
        {
            // Önce Discount API'den rate'i çek
            var discountResult = await discountService.GetDiscountByCodeAsync(coupon);
            if (discountResult.IsFail)
            {
                Message = "Geçersiz kupon kodu veya kupon süresi dolmuş!";
                return RedirectToPage();
            }

            // Rate'i alıp Basket API'ye uygula
            var result = await basketService.ApplyDiscountAsync(discountResult.Data.Code, discountResult.Data.Rate);
            if (result.IsFail)
            {
                Message = "İndirim kuponu uygulanırken bir hata oluştu.";
            }
            else
            {
                Message = $"{discountResult.Data.Code} kuponu başarıyla uygulandı! %{discountResult.Data.Rate * 100:N0} indirim";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveDiscountAsync()
        {
            var result = await basketService.RemoveDiscountAsync();
            if (result.IsFail)
            {
                Message = "İndirim kaldırılırken bir hata oluştu.";
            }
            else
            {
                Message = "İndirim başarıyla kaldırıldı.";
            }
            return RedirectToPage();
        }
    }
}
