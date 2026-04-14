using MicroserviceWebApp.Pages.Basket.ViewModel;
using MicroserviceWebApp.Pages.Order.ViewModel;
using MicroserviceWebApp.Services;
using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Basket
{
    [Authorize]
    public class CheckoutModel(BasketService basketService, OrderService orderService) : PageModel
    {
        [BindProperty]
        public CreateOrderViewModel OrderInput { get; set; } = new()
        {
            // Mock data for easy testing
            Address = new AddressInputViewModel
            {
                Province = "İstanbul",
                City = "Kadıköy",
                Region = "Moda",
                Street = "Atatürk Caddesi No:123",
                PostalCode = "34710"
            },
            Payment = new PaymentInputViewModel
            {
                CardName = "Test User",
                CardNumber = "4111111111111111", // Test Visa card
                Expiration = "12/25",
                Cvv = "123",
                Amount = 0 // Will be set from basket
            }
        };

        public BasketViewModel Basket { get; set; } = default!;

        [TempData]
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var basketResult = await basketService.GetBasket();

            if (basketResult.IsFail)
            {
                Message = "Sepet bilgileri alınamadı.";
                return RedirectToPage("/Basket/Basket");
            }

            Basket = basketResult.Data!;

            if (Basket.IsEmpty)
            {
                Message = "Sepetiniz boş. Lütfen önce ürün ekleyin.";
                return RedirectToPage("/Basket/Basket");
            }

            // Set payment amount from basket
            OrderInput.Payment.Amount = Basket.TotalPrice;
            OrderInput.Discount = Basket.HasDiscount ? (Basket.Discount!.Rate * 100) : 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var basketResult = await basketService.GetBasket();
                if (basketResult.IsSuccess)
                {
                    Basket = basketResult.Data!;
                }
                return Page();
            }

            // Get basket for order items
            var currentBasketResult = await basketService.GetBasket();
            if (currentBasketResult.IsFail)
            {
                Message = "Sepet bilgileri alınamadı.";
                return Page();
            }

            var currentBasket = currentBasketResult.Data!;

            // Create order request
            var orderRequest = new CreateOrderRequest(
                Discount: OrderInput.Discount,
                Address: new AddressDto(
                    OrderInput.Address.Province,
                    OrderInput.Address.City,
                    OrderInput.Address.Region,
                    OrderInput.Address.Street,
                    OrderInput.Address.PostalCode
                ),
                Payment: new PaymentDto(
                    OrderInput.Payment.CardName,
                    OrderInput.Payment.CardNumber,
                    OrderInput.Payment.Expiration,
                    OrderInput.Payment.Cvv,
                    OrderInput.Payment.Amount
                ),
                Orders: currentBasket.Items.Select(item => new OrderItemDto(
                    item.CourseId,
                    item.CourseName,
                    item.CoursePrice
                )).ToList()
            );

            var result = await orderService.CreateOrderAsync(orderRequest);

            if (result.IsFail)
            {
                Message = "Sipariş oluşturulurken bir hata oluştu.";
                Basket = currentBasket;
                return Page();
            }

            var orderResponse = result.Data!;

            // Discount code'unu PaymentSuccess sayfasına taşı
            if (currentBasket.HasDiscount)
            {
                TempData["DiscountCode"] = currentBasket.Discount!.Coupon;
            }

            // Redirect based on payment status
            return orderResponse.PaymentStatus switch
            {
                "Paid" => RedirectToPage("/Basket/PaymentSuccess", new { orderId = orderResponse.OrderId }),
                "Pending" => RedirectToPage("/Basket/PaymentPending", new { orderId = orderResponse.OrderId }),
                "Failed" => RedirectToPage("/Basket/PaymentFailed", new { orderId = orderResponse.OrderId }),
                _ => RedirectToPage("/Basket/PaymentFailed", new { orderId = orderResponse.OrderId })
            };
        }
    }
}
