namespace MicroserviceWebApp.Options
{
    public class MicroserviceOptions
    {
        public required MicroServiceOptionItem Catalog { get; set; }
        public required MicroServiceOptionItem Basket { get; set; }
        public required MicroServiceOptionItem Order { get; set; }
        public required MicroServiceOptionItem File { get; set; }
        public required MicroServiceOptionItem Discount { get; set; }
    }
    public class MicroServiceOptionItem
    {
        public required string BaseAddress
        {
            get; set;
        }
    }
}
