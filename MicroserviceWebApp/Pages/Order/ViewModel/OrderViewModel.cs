namespace MicroserviceWebApp.Pages.Order.ViewModel
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public decimal Discount { get; set; }
        public AddressViewModel Address { get; set; } = default!;
        public List<OrderItemViewModel> OrderItems { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = default!;

        // Calculated Properties
        public decimal SubTotal => OrderItems.Sum(x => x.TotalPrice);
        public decimal DiscountAmount => SubTotal * (Discount / 100);
        public decimal FinalPrice => SubTotal - DiscountAmount;
        public int ItemCount => OrderItems.Count;

        // Formatted Properties
        public string FormattedSubTotal => $"${SubTotal:N2}";
        public string FormattedDiscount => Discount > 0 ? $"{Discount}%" : "İndirim Yok";
        public string FormattedDiscountAmount => $"-${DiscountAmount:N2}";
        public string FormattedFinalPrice => $"${FinalPrice:N2}";
        public string FormattedCreatedDate => CreatedDate.ToString("dd MMMM yyyy HH:mm");
        public string FormattedStatus => Status switch
        {
            "Pending" => "Beklemede",
            "Processing" => "İşleniyor",
            "Completed" => "Tamamlandı",
            "Cancelled" => "İptal Edildi",
            _ => Status
        };
    }
}
