using Microservice.Order.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Application.Contracts.Repositories
{
    public interface IOrderRepository:IGenericRepository<Guid,Domain.Entitites.Order>
    {
        Task<List<Domain.Entitites.Order>> GetOrderBuyerId(Guid buyerId);
        Task SetStatus(string orderCode,Guid paymentId ,OrderStatus status);
    }
}
