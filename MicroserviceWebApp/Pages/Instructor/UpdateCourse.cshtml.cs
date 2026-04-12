using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroserviceWebApp.Pages.Instructor.Dto;
using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace MicroserviceWebApp.Pages.Instructor
{
    [Authorize(Roles = "instructor")]
    public class UpdateCourseModel(CatalogService catalogService) : PageModel
    {
        [BindProperty]
        public UpdateCourseViewModel Input { get; set; } = UpdateCourseViewModel.Empty;

        [TempData]
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            // Kurs bilgilerini getir
            var courseResult = await catalogService.GetCourseByIdAsync(id);
            if (courseResult.IsFail)
            {
                Message = "Kurs bulunamadı.";
                return RedirectToPage("/Instructor/Courses");
            }

            // Kategorileri getir
            var categoriesResult = await catalogService.GetCategoriesAsync();
            if (categoriesResult.IsFail)
            {
                Message = "Kategoriler yüklenirken bir hata oluştu.";
                return RedirectToPage("/Instructor/Courses");
            }

            // ViewModel'i doldur
            var course = courseResult.Data!;
            Input = new UpdateCourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                ImageUrl = course.ImageUrl,
                CategoryId = course.Category.Id
            };
            Input.SetCategoryDropdownList(categoriesResult.Data!);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categoriesResult = await catalogService.GetCategoriesAsync();
                if (!categoriesResult.IsFail)
                {
                    Input.SetCategoryDropdownList(categoriesResult.Data!);
                }
                return Page();
            }

            var request = new UpdateCourseRequest(
                Input.Id,
                Input.Name,
                Input.Description,
                Input.Price,
                Input.ImageUrl,
                Input.CategoryId
            );

            var result = await catalogService.UpdateCourseAsync(request);
            if (result.IsFail)
            {
                Message = "Kurs güncellenirken bir hata oluştu.";
                var categoriesResult = await catalogService.GetCategoriesAsync();
                if (!categoriesResult.IsFail)
                {
                    Input.SetCategoryDropdownList(categoriesResult.Data!);
                }
                return Page();
            }

            Message = "Kurs başarıyla güncellendi.";
            return RedirectToPage("/Instructor/Courses");
        }
    }
}
