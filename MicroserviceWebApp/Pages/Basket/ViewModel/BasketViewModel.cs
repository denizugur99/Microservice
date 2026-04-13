namespace MicroserviceWebApp.Pages.Basket.ViewModel
{
    public class BasketViewModel
    {
        public string UserId { get; set; } = default!;
        public List<BasketItemViewModel> Items { get; set; } = new();
        public BasketDiscountViewModel? Discount { get; set; }

        // Calculated Properties
        public decimal SubTotal => Items.Sum(x => x.CoursePrice);
        public decimal DiscountAmount => Discount != null ? SubTotal * Discount.Rate : 0;
        public decimal TotalPrice => SubTotal - DiscountAmount;
        public int ItemCount => Items.Count;
        public bool HasDiscount => Discount != null;
        public bool IsEmpty => Items.Count == 0;

        // Formatted Properties
        public string FormattedSubTotal => $"${SubTotal:N2}";
        public string FormattedDiscountAmount => $"-${DiscountAmount:N2}";
        public string FormattedTotalPrice => $"${TotalPrice:N2}";
        public string FormattedDiscountRate => Discount != null ? $"{Discount.Rate * 100:N0}%" : "0%";
    }
}
