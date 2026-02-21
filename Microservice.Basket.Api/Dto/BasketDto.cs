using System.Text.Json.Serialization;

namespace Microservice.Basket.Api.Dto
{
    public record BasketDto
    {
      
        public List<BasketItemDto> Items { get; init; } = new();

        public float? DiscountRate { get; set; }
        public string? Coupon { get; set; }

        [JsonIgnore]
        public bool IsApplyDiscount => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon);
        public decimal TotalPrice => Items.Sum(x => x.Price);
       
        public decimal? TotalPriceWithDiscount
        {
            get
            {
                if (!IsApplyDiscount)
                {
                    return TotalPrice;
                }
                return Items.Sum(x => x.PriceByApplyDiscountRate);
            }

        }

        public BasketDto(List<BasketItemDto> BasketItems) 
        {
           
            Items = BasketItems;
        }
        public BasketDto() { }
        
    }
  
      
    
}
