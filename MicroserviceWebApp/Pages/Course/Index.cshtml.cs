using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Course
{
    public class CourseIndexModel(CatalogService catalogService) : PageModel
    {
        public List<CourseViewModel> CoursesViewModel { get; set; } = null!;

        public async Task OnGetAsync()
        {
            var result = await catalogService.GetAllCoursesAsync();
            if (result.IsFail)
            {
                //TODO: hata sayfasına yönlendirme yapılacak
            }
            else
            {
                CoursesViewModel = result.Data!;
            }
        }
    }
}
