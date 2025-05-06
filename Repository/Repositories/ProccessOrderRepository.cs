using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Coconseconsentext;
using Repository.Enums;
using Repository.Interface;


namespace Repository.Repositories
{
   public class ProccessOrderRepository: Repository<Order, OrdersDb>, IProcessOrderRepository
    {
        public ProccessOrderRepository(OrdersDb context): base(context) { }

        public async Task<Order> CreateOrder(Order order)
        {            
            var result = await AddAsync(order);
            return result;
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            var result = await UpdateAsync(order);
            return result;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var result = await GetByIdAsync(orderId, p => p.Include(p => p.Client).Include(p => p.OrderItems).ThenInclude(p=> p.Item));                      
            return result;
        }

    }
}
