using Application.Dto;
using Application.Interface;
using Domain.Models;
using Repository.Interface;


namespace Application.Service
{
    public class CalculateOrderService : ICalculateOrderService
    {       
        private readonly IItemRepository _itemRepository;

        public CalculateOrderService(IItemRepository itemRepository)
        {           
            _itemRepository = itemRepository;
        }

        public async Task<Order> CalculateOrder(Order order, List<ItemsDto> orderItems)
        {
            decimal subtotal =0;
            //Consultar todos los items existentes
            var items = await _itemRepository.GetAllAsync();

            // Calculamos el subtotal, iva y total           
            foreach (var item in orderItems)
            {
                subtotal += items.Find(p => p.IdItem == item.IdItem).PriceUnit * item.Ammount;
            }

            order.SubTotal = subtotal;
            order.Iva = (subtotal * 0.19m);
            order.Total = subtotal + order.Iva;
           
          return order;            
        }

    }
}
