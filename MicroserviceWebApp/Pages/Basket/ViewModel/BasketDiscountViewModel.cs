namespace MicroserviceWebApp.Pages.Basket.ViewModel
{
    public class BasketDiscountViewModel
    {
        public string Coupon { get; set; } = default!;
        public decimal Rate { get; set; }

        // Formatted Properties
        public string FormattedRate => $"{Rate * 100:N0}%";
        public string FormattedCoupon => Coupon.ToUpper();
    }
}
