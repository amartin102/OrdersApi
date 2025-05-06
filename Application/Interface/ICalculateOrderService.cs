using Application.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICalculateOrderService
    {
        Task<Order> CalculateOrder(Order order, List<ItemsDto> items);
    }
}
