using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Instructor.ViewModel
{
    public class UpdateCourseViewModel
    {
        public static UpdateCourseViewModel Empty => new();

        public Guid Id { get; set; }

        [Display(Name = "Course Category")]
        public SelectList CategoryDropdownList { get; set; } = default!;

        [Required(ErrorMessage = "Kurs adı zorunludur")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Kurs adı 3-100 karakter arasında olmalıdır")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Açıklama 10-500 karakter arasında olmalıdır")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, 999999, ErrorMessage = "Fiyat 0.01 ile 999999 arasında olmalıdır")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public Guid CategoryId { get; set; }

        public void SetCategoryDropdownList(List<CategoryViewModel> categories)
        {
            CategoryDropdownList = new SelectList(categories, "Id", "Name", CategoryId);
        }
    }
}
