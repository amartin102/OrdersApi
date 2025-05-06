using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
   public interface IProcessOrderRepository //: IRepository<Order>
    {                
       Task<Order> CreateOrder (Order order);

        Task<bool> UpdateOrder (Order order);

        Task<Order> GetOrderById(int orderId);

    }
}
