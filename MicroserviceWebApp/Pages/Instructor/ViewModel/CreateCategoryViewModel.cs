using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Instructor.ViewModel
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Kategori adı 2-50 karakter arasında olmalıdır")]
        public string Name { get; set; } = default!;
    }
}
