using Microservice.Order.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.Persistence.Repositories
{
    public class OrderRepository(AppDbContext _context):GenericRepository<Guid, Domain.Entitites.Order>(_context),IOrderRepository 
    {
        public Task<List<Domain.Entitites.Order>> GetOrderBuyerId(Guid buyerId)
        {
            return _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == buyerId).OrderByDescending(x => x.Created).ToListAsync();
        }
    }
}
