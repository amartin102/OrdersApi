using Domain.Models;
using Repository.Coconseconsentext;
using Repository.Interface;

namespace Repository.Repositories
{
    public class OrderItemRepository : Repository<OrderItem, OrdersDb>, IOrderItemRepository
    {
        public OrderItemRepository(OrdersDb context) : base(context)
        {
        }

        public async Task<List<OrderItem>> CreateOrderItem(List<OrderItem> orderitem)
        {
            var result = await AddRangeAsync(orderitem);
            return result;
        }

        public async Task<bool> InactiveAsync(int idOrderItem)
        {
            var result = await DeleteAsync(idOrderItem);
            return result;
        }

        public async Task<bool> InactiveAllAsync(int idOrder)
        { 
            var result = await DeleteAllAsync(idOrder);
            return result;
        }
    }
}
