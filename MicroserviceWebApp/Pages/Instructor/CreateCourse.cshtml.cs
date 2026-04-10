using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroserviceWebApp.Pages.Instructor.Dto;
using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace MicroserviceWebApp.Pages.Instructor
{
    [Authorize(Roles ="instructor")]
    public class CreateCourseModel(CatalogService catalogService) : PageModel
    {
        [BindProperty]
        public CreateCourseViewModel Input { get; set; } = CreateCourseViewModel.Empty;

        public async Task OnGet()
        {
           var catalogResult =  await catalogService.GetCategoriesAsync();

            if (catalogResult.IsFail)
            {
                //TODO : hata sayfasına yönlendirme yapılacak
            }
            Input.SetCategoryDropdownList(catalogResult.Data!);
        }

        public async Task<IActionResult> OnPostAsync()
        {
         var result =   await catalogService.CreateCourseAsync(Input);
            if (result.IsFail)
            {
                //TODO : hata sayfasına yönlendirme yapılacak
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
