using AutoMapper;
using Microservice.Basket.Api.Data;
using Microservice.Basket.Api.Dto;

namespace Microservice.Basket.Api.Features.Baskets
{
    public class BasketMapping:Profile
    {
        public BasketMapping() {
            CreateMap<BasketDto, Data.Basket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        }
    }
}
