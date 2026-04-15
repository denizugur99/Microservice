using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Course
{
    public class CourseDetailsModel(CatalogService catalogService, BasketService basketService, UserService userService) : PageModel
    {
        public CourseViewModel? Course { get; set; }
        public Guid? CurrentUserId { get; set; }

        [TempData]
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var result = await catalogService.GetCourseByIdAsync(id);
            if (result.IsFail)
            {
                //TODO: hata sayfasına yönlendirme yapılacak
                return RedirectToPage("/Index");
            }

            Course = result.Data;

            if (User.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = userService.GetUserId;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToBasketAsync(Guid courseId, string courseName, decimal coursePrice, string imageUrl, Guid userId)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Auth/SignIn");
            }

            var currentUserId = userService.GetUserId;

            // Kullanıcının kendi kursunu sepete eklemeye çalışıp çalışmadığını kontrol et
            if (currentUserId == userId)
            {
                Message = "Kendi kursunuzu sepete ekleyemezsiniz!";
                return RedirectToPage(new { id = courseId });
            }

            var request = new AddBasketItemRequest(courseId, courseName, coursePrice, imageUrl);
            var result = await basketService.AddItemToBasketAsync(request);

            if (result.IsFail)
            {
                Message = "Ürün sepete eklenirken bir hata oluştu.";
                return RedirectToPage(new { id = courseId });
            }

            Message = "Ürün başarıyla sepete eklendi!";
            return RedirectToPage("/Basket/Basket");
        }
    }
}
