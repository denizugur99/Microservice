namespace MicroserviceWebApp.Options
{
    public class MicroserviceOptions
    {
        public required MicroServiceOptionItem Catalog { get; set; }
        public required MicroServiceOptionItem File { get; set; }
    }
    public class MicroServiceOptionItem
    {
        public required string BaseAddress
        {
            get; set;
        }
    }
}
