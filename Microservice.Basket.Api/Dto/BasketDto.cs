using System.Text.Json.Serialization;

namespace Microservice.Basket.Api.Dto
{
    public record BasketDto
    {
       [JsonIgnore] public Guid UserId { get; init; }
        public List<BasketItemDto> Items { get; init; } = new();

        public BasketDto(Guid userId,List<BasketItemDto> BasketItems) 
        {
            UserId = userId;
            Items = BasketItems;
        }
        public BasketDto() { }
        
    }
  
      
    
}
