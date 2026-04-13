using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages
{
    public class IndexModel(CatalogService catalogService, BasketService basketService) : PageModel
    {
        public List<CourseViewModel> CoursesViewModel { get; set; } = null!;

        [TempData]
        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            var result = await catalogService.GetAllCoursesAsync();
            if (result.IsFail)
            {
                //TODO:hata yönlendirmesi
            }
            else
            {
                CoursesViewModel = result.Data!;
            }
        }

        public async Task<IActionResult> OnPostAddToBasketAsync(Guid courseId, string courseName, decimal coursePrice, string imageUrl)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Auth/SignIn");
            }

            var request = new AddBasketItemRequest(courseId, courseName, coursePrice, imageUrl);
            var result = await basketService.AddItemToBasketAsync(request);

            if (result.IsFail)
            {
                Message = "Ürün sepete eklenirken bir hata oluştu.";
            }
            else
            {
                Message = "Ürün başarıyla sepete eklendi!";
            }

            return RedirectToPage();
        }
    }
}
