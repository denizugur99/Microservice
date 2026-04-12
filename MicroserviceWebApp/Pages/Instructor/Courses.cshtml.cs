using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Instructor
{
    public class CoursesModel(CatalogService catalogService) : PageModel
    {
        public List<CourseViewModel> CoursesViewModel { get; set; }=null!;

        [TempData]
        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            var result = await catalogService.GetCoursesAsync();
            if (result.IsFail)
            {
                //TODO : hata sayfasına yönlendirme yapılacak

            }
            else
            {
                CoursesViewModel = result.Data!;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid courseId)
        {
            var result = await catalogService.DeleteCourseAsync(courseId);

            if (result.IsFail)
            {
                Message = "Kurs silinirken bir hata oluştu.";
            }
            else
            {
                Message = "Kurs başarıyla silindi.";
            }

            return RedirectToPage();
        }
    }
}
