using Application.Dto;
using Application.Interface;
using Confluent.Kafka;
using Domain.Models;
using ExternalServices.Common;
using ExternalServices.Consumer;
using ExternalServices.KafkaConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repository.Enums;
using Repository.Interface;
using System.Threading;
using System.Transactions;

namespace Application.Service
{
    public class ProcessOrderService : IProcessOrderService
    {
        private readonly IProcessOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICalculateOrderService _calculateOrderService;
        private readonly IProducer _producerBus;
        private readonly IOptions<KafkaSettings> _kafkaSettings;
        private readonly ILogger<ProcessOrderDto> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProcessOrderService(IProcessOrderRepository orderRepository, IOrderItemRepository orderItemRepository, ICalculateOrderService calculateOrderService, IProducer producer, IOptions<KafkaSettings> kafkaSettings, ILogger<ProcessOrderDto> logger, IServiceProvider serviceProvider)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _calculateOrderService = calculateOrderService;
            _producerBus = producer;
            _kafkaSettings = kafkaSettings;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public async Task<bool> CreateOrder(ProcessOrderDto order)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                List<OrderItem> items = new();

                var calculatedOrder = CalculateOrder(order);
                calculatedOrder.Result.CreationDate = DateTime.Now;
                calculatedOrder.Result.IdClient = order.IdClient;

                //Creamos el encabezado
                var headerOrder = await _orderRepository.CreateOrder(calculatedOrder.Result);

                if (headerOrder != null)
                {
                    foreach (var item in order.items)
                    {
                        items.Add(new OrderItem { IdOrder = headerOrder.IdOrder, IdItem = item.IdItem, Ammount = item.Ammount, Active = true });
                    }

                    //Creamos el detalle
                    if (await _orderItemRepository.CreateOrderItem(items) != null)
                    {
                        List<int> recipes = new List<int>();

                        foreach (var recipe in items) {
                            recipes.Add(recipe.Item.IdRecipe);
                        }

                        var key = Guid.NewGuid();
                        _logger.LogInformation($"Validando disponibilidad para el Pedido nro: {headerOrder.IdOrder}");

                        //Enviar el mensaje de disponibilidad
                        var eventMessagge = new CheckAvailabilityRequestEvent(key, headerOrder.IdOrder, recipes);
                        await _producerBus.SendAsync(_kafkaSettings.Value.CheckAvailabilityRequestTopic, eventMessagge);
                        _logger.LogInformation("Mensaje enviado: consultar disponibilidad.");
                        
                        _logger.LogInformation("Consumir mensaje en proceso..");

                        //Consumir la respuesta de disponibilidad
                        using (IServiceScope scope = _serviceProvider.CreateScope())
                        {
                            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
                            var result = await eventConsumer.Consume(_kafkaSettings.Value.CheckAvailabilityResponseTopic, key.ToString());

                            _logger.LogInformation($"Mensaje disponibilidad validada, consumido: {result}");

                            if (result?.IsAvailable == true)
                            {
                                headerOrder.Status = (int)OrderStatus.Confirmed;
                                await _orderRepository.UpdateOrder(headerOrder);
                                _logger.LogInformation($"Pedido credo, disponibilidad de stock.");

                                await _producerBus.SendAsync(_kafkaSettings.Value.CreatedOrderTopic,
                                                              new CreatedOrderEvent(Guid.NewGuid(), $"Pedido nro: {headerOrder.IdOrder} creado", headerOrder.IdOrder));
                                _logger.LogInformation("Mensaje pedido creado, enviado.");

                                transactionScope.Complete();
                                return true;
                            }
                        }                        
                                              
                    }
                }

                transactionScope.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                transactionScope.Dispose();
                throw;
            }
        }

        public async Task<OrderDto> GetOrderById(int IdOrder)
        {
            OrderDto orderDto = new OrderDto();
            var result = await _orderRepository.GetOrderById(IdOrder);

            if (result != null) //Seteamos los campos al DTO
            {
                orderDto.IdOrder = result.IdOrder;
                orderDto.ClientName = $"{result.Client.FirstName} {result.Client.LastName}";
                orderDto.Address = result.Client.Address;
                orderDto.Total = result.Total;
                orderDto.Iva = result.Iva.ToString();
                orderDto.SubTotal = result.SubTotal.ToString();
                orderDto.Status = result.Status;

                foreach (var orderItem in result.OrderItems)
                {
                    orderDto.items.Add(new ItemDto { IdItem = orderItem.IdItem, Description = orderItem.Item.Description, PriceUnit = orderItem.Item.PriceUnit, Ammount = orderItem.Ammount, RecipeId = orderItem.Item.IdRecipe });
                }
            }
            
            return orderDto;
        }

        public async Task<bool> UpdateOrder(ProcessOrderDto order)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {                
                //1. Consultar el encabezado de la orden
                var headerOrder = await _orderRepository.GetOrderById(order.IdOrder);

                //2. Calculamos la orden y actualizamos el encabezado
                var calculatedOrder = CalculateOrder(order);

                headerOrder.Iva = calculatedOrder.Result.Iva;
                headerOrder.SubTotal = calculatedOrder.Result.SubTotal;
                headerOrder.Total = calculatedOrder.Result.Total;
                headerOrder.ModificationDate = calculatedOrder.Result.ModificationDate;

                if (await _orderRepository.UpdateOrder(headerOrder))
                {
                    //3. Inactivar los items existentes
                    if (await _orderItemRepository.InactiveAllAsync(order.IdOrder))
                    {
                        //4. Crear items nuevos
                        List<OrderItem> items = new();

                        foreach (var item in order.items)
                        {
                            items.Add(new OrderItem { IdOrder = order.IdOrder, IdItem = item.IdItem, Ammount = item.Ammount, Active = true });
                        }

                        //Creamos el detalle
                        if (await _orderItemRepository.CreateOrderItem(items) != null)
                        {
                            scope.Complete();
                            return true;
                        }
                    }
                }

                scope.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                scope.Dispose();
                return false;
                throw;
            }
        }

        private async Task<Order> CalculateOrder(ProcessOrderDto order)
        {
            //Calculamos la orden antes de crearla
            var calculatedOrder = await _calculateOrderService.CalculateOrder(new Order(), order.items);
                       
            calculatedOrder.Status = (int)OrderStatus.Pending;           
            calculatedOrder.ModificationDate = DateTime.Now;

            return calculatedOrder;
        }
    }
}
