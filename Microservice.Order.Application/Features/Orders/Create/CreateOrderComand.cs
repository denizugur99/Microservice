using Microservice.Order.Domain.Entitites;
using Microservices.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Features.Orders.Create
{
    public record CreateOrderComand(float? Discount, AddressDto Address, PaymentDto Payment, List<OrderItemDto> Orders) : IrequestByServiceResult<CreateOrderResponse>;

    public record AddressDto(string Province, string City,string Region, string Street, string PostalCode);

    public record PaymentDto(string CardName, string CardNumber, string Expiration, string CVV,decimal Amount);
    public record OrderItemDto(Guid ProductId,string ProductName, decimal UnitPrice);

    public record CreateOrderResponse(Guid OrderId, string PaymentStatus);
}
