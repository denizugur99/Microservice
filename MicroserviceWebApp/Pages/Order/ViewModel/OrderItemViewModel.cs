namespace MicroserviceWebApp.Pages.Order.ViewModel
{
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;

        // Calculated Properties
        public decimal TotalPrice => UnitPrice * Quantity;

        // Formatted Properties
        public string FormattedUnitPrice => $"${UnitPrice:N2}";
        public string FormattedTotalPrice => $"${TotalPrice:N2}";
    }
}
