using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
   public interface IOrderItemRepository
    {
        Task<List<OrderItem>> CreateOrderItem(List<OrderItem> orderItem);

        Task<bool> InactiveAsync(int idOrderItem);

        Task<bool> InactiveAllAsync(int idOrder);

    }
}
