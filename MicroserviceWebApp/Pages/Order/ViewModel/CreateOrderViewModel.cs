using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Order.ViewModel
{
    public class CreateOrderViewModel
    {
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Adres bilgileri gereklidir")]
        public AddressInputViewModel Address { get; set; } = default!;

        [Required(ErrorMessage = "Ödeme bilgileri gereklidir")]
        public PaymentInputViewModel Payment { get; set; } = default!;

        public List<OrderItemViewModel> OrderItems { get; set; } = new();
    }

    public class AddressInputViewModel
    {
        [Required(ErrorMessage = "İl gereklidir")]
        public string Province { get; set; } = default!;

        [Required(ErrorMessage = "İlçe gereklidir")]
        public string City { get; set; } = default!;

        [Required(ErrorMessage = "Bölge gereklidir")]
        public string Region { get; set; } = default!;

        [Required(ErrorMessage = "Sokak bilgisi gereklidir")]
        public string Street { get; set; } = default!;

        [Required(ErrorMessage = "Posta kodu gereklidir")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Geçerli bir posta kodu giriniz (5 haneli)")]
        public string PostalCode { get; set; } = default!;
    }

    public class PaymentInputViewModel
    {
        [Required(ErrorMessage = "Kart sahibi adı gereklidir")]
        public string CardName { get; set; } = default!;

        [Required(ErrorMessage = "Kart numarası gereklidir")]
        [CreditCard(ErrorMessage = "Geçerli bir kart numarası giriniz")]
        public string CardNumber { get; set; } = default!;

        [Required(ErrorMessage = "Son kullanma tarihi gereklidir")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Format: AA/YY (örn: 12/25)")]
        public string Expiration { get; set; } = default!;

        [Required(ErrorMessage = "CVV gereklidir")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV 3 veya 4 haneli olmalıdır")]
        public string Cvv { get; set; } = default!;

        [Required(ErrorMessage = "Tutar gereklidir")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tutar 0'dan büyük olmalıdır")]
        public decimal Amount { get; set; }

        // Masked display
        public string MaskedCardNumber => CardNumber?.Length >= 4
            ? $"**** **** **** {CardNumber.Substring(CardNumber.Length - 4)}"
            : "****";
    }
}
