using MicroserviceWebApp.Pages.Instructor.Dto;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Course
{
    public class CourseDetailsModel(CatalogService catalogService) : PageModel
    {
        public CourseDto? Course { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var result = await catalogService.GetCourseByIdAsync(id);
            if (result.IsFail)
            {
                //TODO: hata sayfasına yönlendirme yapılacak
                return RedirectToPage("/Course/Index");
            }

            Course = result.Data;
            return Page();
        }
    }
}
