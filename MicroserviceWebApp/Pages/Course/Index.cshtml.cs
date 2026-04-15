using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using MicroserviceWebApp.Services.Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Course
{
    public class CourseIndexModel(CatalogService catalogService, BasketService basketService, UserService userService) : PageModel
    {
        public List<CourseViewModel> CoursesViewModel { get; set; } = null!;
        public Guid? CurrentUserId { get; set; }

        [TempData]
        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            var result = await catalogService.GetAllCoursesAsync();
            if (result.IsFail)
            {
                //TODO: hata sayfasına yönlendirme yapılacak
                CoursesViewModel = new List<CourseViewModel>();
                return;
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = userService.GetUserId;
            }

            CoursesViewModel = result.Data!;

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
                return RedirectToPage();
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
