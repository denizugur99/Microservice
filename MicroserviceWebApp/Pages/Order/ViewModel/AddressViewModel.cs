namespace MicroserviceWebApp.Pages.Order.ViewModel
{
    public class AddressViewModel
    {
        public string Province { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Region { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string PostalCode { get; set; } = default!;

        // Formatted Properties
        public string FullAddress => $"{Street}, {City}/{Province}, {Region} - {PostalCode}";
        public string ShortAddress => $"{City}/{Province}";
    }
}
