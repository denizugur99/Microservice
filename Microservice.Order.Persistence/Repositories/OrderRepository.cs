using Microservice.Order.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Persistence.Repositories
{
    public class OrderRepository(AppDbContext context):GenericRepository<Guid, Order.Domain.Entitites.Order>(context),IOrderRepository 
    {
    }
}
