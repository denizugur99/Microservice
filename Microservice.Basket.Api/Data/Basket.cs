using System.Text.Json.Serialization;

namespace Microservice.Basket.Api.Data
{
    public class Basket
    {
        public Guid UserId { get; set; }
        public List<BasketItem> Items { get; set; } = new();
        public float? DiscountRate { get; set; }
        public string? Coupon { get; set; }

        [JsonIgnore]
        public bool IsApplyDiscount => DiscountRate is > 0 && !string.IsNullOrEmpty(Coupon);
        [JsonIgnore]
        public decimal TotalPrice => Items.Sum(x => x.Price);
        [JsonIgnore]
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
        public Basket(Guid userId, List<BasketItem> items)
        {
            UserId = userId;
            Items = items;
        }
        public Basket() { }
        public void ApplyNewDiscount(string coupon, float discountRate)
        {
            Coupon = coupon;
            DiscountRate = discountRate;
            foreach (var item in Items)
            {
                item.PriceByApplyDiscountRate=item.Price*(decimal)(1-discountRate);

            }
        }
        public void ApplyAvailableDiscount()
        {
            if(!IsApplyDiscount)
                return;
         
            foreach (var item in Items)
            {
                item.PriceByApplyDiscountRate = item.Price * (decimal)(1 - DiscountRate!);

            }
        }
        public void ClearDiscount() {
            DiscountRate = null;
            Coupon=null;
            foreach (var item in Items) {
            item.PriceByApplyDiscountRate=null;
            }
        
        }
    }
}