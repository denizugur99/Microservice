using MicroserviceWebApp.Pages.Instructor.ViewModel;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroserviceWebApp.Pages.Instructor
{
    public class DashboardModel(CatalogService catalogService) : PageModel
    {
        // İstatistikler
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; } = 142; // Örnek veri
        public string TotalEarnings { get; set; } = "24,580"; // Örnek veri
        public string AverageRating { get; set; } = "4.8";
        public int TotalReviews { get; set; } = 89; // Örnek veri

        // Son eklenen kurslar
        public List<CourseViewModel>? RecentCourses { get; set; }

        public async Task OnGetAsync()
        {
            var result = await catalogService.GetCoursesAsync();

            if (result.IsSuccess)
            {
                var courses = result.Data ?? new List<CourseViewModel>();

                // İstatistikleri hesapla
                TotalCourses = courses.Count;

                // Son 5 kursu al
                RecentCourses = courses
                    .OrderByDescending(c => c.Id) // Yeni eklenenler önce
                    .Take(5)
                    .ToList();

                // Ortalama rating hesapla (eğer kurslar varsa)
                if (courses.Any())
                {
                    var avgRating = courses.Average(c => c.Rating);
                    AverageRating = avgRating.ToString("F1");
                }
            }
            else
            {
                // Hata durumunda boş liste
                RecentCourses = new List<CourseViewModel>();
                TotalCourses = 0;
            }
        }
    }
}
