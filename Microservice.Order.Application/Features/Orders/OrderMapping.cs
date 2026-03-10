using AutoMapper;
using Microservice.Order.Application.Features.Orders.Create;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Features.Orders
{
    public class OrderMapping:Profile
    {
        public OrderMapping() {
        CreateMap<Domain.Entitites.Order,OrderItemDto>().ReverseMap();
        }
    }
}
