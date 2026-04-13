namespace MicroserviceWebApp.Pages.Basket.ViewModel
{
    public class BasketItemViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = default!;
        public decimal CoursePrice { get; set; }
        public string ImageUrl { get; set; } = default!;

        // Formatted Properties
        public string FormattedPrice => $"${CoursePrice:N2}";
    }
}
