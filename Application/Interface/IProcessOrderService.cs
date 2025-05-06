using Application.Dto;
using Domain.Models;

namespace Application.Interface
{
    public interface IProcessOrderService
    {
        Task<bool> CreateOrder(ProcessOrderDto order);

        Task<bool> UpdateOrder(ProcessOrderDto order);

        Task<OrderDto> GetOrderById(int IdOrder);
    }
}
