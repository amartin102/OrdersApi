using Domain.Models;
using Repository.Coconseconsentext;
using Repository.Interface;

namespace Repository.Repositories
{
    public class ItemRepository : Repository<Item, OrdersDb>, IItemRepository
    {
        public ItemRepository(OrdersDb context) : base(context)
        {
        }

        public async Task<List<Item>> GetAllOrders()
        {
            var result = await GetAllAsync();
            return result;
        }

    }
}
