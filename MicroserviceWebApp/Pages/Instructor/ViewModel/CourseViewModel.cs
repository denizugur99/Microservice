namespace MicroserviceWebApp.Pages.Instructor.ViewModel
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public int Duration { get; set; }
        public float Rating { get; set; }
        public string UserName { get; set; } = default!;

        public string FormattedPrice => $"${Price:N2}";
        public string FormattedDuration => Duration > 0 ? $"{Duration} saat" : "Süre belirtilmemiş";
        public string FormattedRating => $"{Rating:F1} ⭐";
    }
}
