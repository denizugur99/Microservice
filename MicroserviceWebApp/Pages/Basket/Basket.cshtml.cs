using MicroserviceWebApp.Pages.Basket.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Basket
{
    [Authorize]
    public class BasketModel(BasketService basketService) : PageModel
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

        public async Task<IActionResult> OnPostApplyDiscountAsync(string coupon, float rate)
        {
            var result = await basketService.ApplyDiscountAsync(coupon, rate);
            if (result.IsFail)
            {
                Message = "İndirim kuponu uygulanırken bir hata oluştu.";
            }
            else
            {
                Message = $"{coupon} kuponu başarıyla uygulandı!";
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
