using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Instructor
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // /Instructor veya /Instructor/Index'e gelenleri Dashboard'a yönlendir
            return RedirectToPage("/Instructor/Dashboard");
        }
    }
}
