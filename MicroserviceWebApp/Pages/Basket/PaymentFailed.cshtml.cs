using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Basket
{
    [Authorize]
    public class PaymentFailedModel : PageModel
    {
        public Guid OrderId { get; set; }

        public IActionResult OnGet(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return RedirectToPage("/Basket/Basket");
            }

            OrderId = orderId;
            return Page();
        }
    }
}
