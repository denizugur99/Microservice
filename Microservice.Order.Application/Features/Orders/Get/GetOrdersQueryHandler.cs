using MediatR;
using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Application.Features.Orders.Create;
using Microservices.Shared;
using Microservices.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Features.Orders.Get
{
    public class GetOrdersQueryHandler(IIdentityService identityService,IOrderRepository orderRepository) : IRequestHandler<GetOrdersQuery, ServiceResult<List<GetOrdersResponse>>>
    {
        public async Task<ServiceResult<List<GetOrdersResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders= await orderRepository.GetOrderBuyerId(identityService.GetUserId);
            var response = orders.Select(x => new GetOrdersResponse(
                OrderDate: x.Created,
                TotalPrice: x.TotalPrice,
                OrderItems: x.OrderItems.Select(y => new OrderItemDto(
                    ProductId: y.ProductId,
                    ProductName: y.ProductName,
                    UnitPrice: y.UnitPrice
                )).ToList()
            )).ToList();

            return ServiceResult<List<GetOrdersResponse>>.SuccesAsOkay(response);
        }
    }
}
