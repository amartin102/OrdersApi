using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
   public interface IItemRepository //: IRepository<Order>
    {   
        Task<List<Item>> GetAllAsync();
    }
}
