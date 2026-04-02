using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Microservice.Order.Persistence.Repositories
{
    public class OrderRepository(AppDbContext _context):GenericRepository<Guid, Domain.Entitites.Order>(_context),IOrderRepository 
    {
        public Task<List<Domain.Entitites.Order>> GetOrderBuyerId(Guid buyerId)
        {
            return _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == buyerId).OrderByDescending(x => x.Created).ToListAsync();
        }

        public async Task SetStatus(string orderCode,Guid paymentId, OrderStatus status)
        {
           var order =await _context.Orders.FirstAsync(x => x.OrderCode == orderCode);
           
          order.Status = status;
          order.PaymentId = paymentId;
            _context.Orders.Update(order);
               
       
            
        }
    }
}
