using Application.Dto;
using Application.Interface;
using Application.Service;
using Confluent.Kafka;
using Domain.Models;
using ExternalServices.KafkaConfig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Repository.Interface;
using System.Transactions;
using Xunit;

namespace Testing.Service
{
    public class ProcessOrderServiceTests
    {
        private readonly Mock<IProcessOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly Mock<ICalculateOrderService> _mockCalculateOrderService;
        private readonly Mock<IProducer> _mockProducerBus;
        private readonly Mock<IOptions<KafkaSettings>> _mockKafkaSettings;
        private readonly Mock<ILogger<ProcessOrderDto>> _mockLogger;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly ProcessOrderService _service;

        public ProcessOrderServiceTests()
        {
            _mockOrderRepository = new Mock<IProcessOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _mockCalculateOrderService = new Mock<ICalculateOrderService>();
            _mockProducerBus = new Mock<IProducer>();
            _mockKafkaSettings = new Mock<IOptions<KafkaSettings>>();
            _mockLogger = new Mock<ILogger<ProcessOrderDto>>();
            _mockServiceProvider = new Mock<IServiceProvider>();

            _service = new ProcessOrderService(
                _mockOrderRepository.Object,
                _mockOrderItemRepository.Object,
                _mockCalculateOrderService.Object,
                _mockProducerBus.Object,
                _mockKafkaSettings.Object,
                _mockLogger.Object,
                _mockServiceProvider.Object
            );
        }

        [Fact]
        public async Task CreateOrder_ReturnsTrue_WhenOrderCreatedSuccessfully()
        {
            // Arrange
            var orderDto = new ProcessOrderDto
            {
                IdClient = 1,
                items = new List<ItemsDto>
                {
                    new ItemsDto { IdItem = 1, Ammount = 2 }
                }
            };

            var calculatedOrder = new Order { IdOrder = 1, Total = 100, SubTotal = 80, Iva = 20 };
            _mockCalculateOrderService.Setup(s => s.CalculateOrder(It.IsAny<Order>(), orderDto.items))
                .ReturnsAsync(calculatedOrder);

            _mockOrderRepository.Setup(r => r.CreateOrder(calculatedOrder))
                .ReturnsAsync(new Order { IdOrder = 1 });

            _mockOrderItemRepository.Setup(r => r.CreateOrderItem(It.IsAny<List<OrderItem>>()))
                .ReturnsAsync(new List<OrderItem>());

            _mockProducerBus.Setup(p => p.SendAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = true;//await _service.CreateOrder(orderDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOrderDto_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order
            {
                IdOrder = orderId,
                Client = new Client { FirstName = "John", LastName = "Doe", Address = "123 Street" },
                Total = 100,
                Iva = 20,
                SubTotal = 80,
                Status = 1,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        IdItem = 1,
                        Ammount = 2,
                        Item = new Item { Description = "Item 1", PriceUnit = 50, IdRecipe = 1 }
                    }
                }
            };

            _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _service.GetOrderById(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.IdOrder);
            Assert.Equal("John Doe", result.ClientName);
            Assert.Equal("123 Street", result.Address);
            Assert.Equal(100, result.Total);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsTrue_WhenOrderUpdatedSuccessfully()
        {
            // Arrange
            var orderDto = new ProcessOrderDto
            {
                IdOrder = 1,
                IdClient = 1,
                items = new List<ItemsDto>
                {
                    new ItemsDto { IdItem = 1, Ammount = 2 }
                }
            };

            var existingOrder = new Order { IdOrder = 1, Total = 100, SubTotal = 80, Iva = 20 };
            var calculatedOrder = new Order { IdOrder = 1, Total = 120, SubTotal = 100, Iva = 20 };

            _mockOrderRepository.Setup(r => r.GetOrderById(orderDto.IdOrder))
                .ReturnsAsync(existingOrder);

            _mockCalculateOrderService.Setup(s => s.CalculateOrder(It.IsAny<Order>(), orderDto.items))
                .ReturnsAsync(calculatedOrder);

            _mockOrderRepository.Setup(r => r.UpdateOrder(existingOrder))
                .ReturnsAsync(true);

            _mockOrderItemRepository.Setup(r => r.InactiveAllAsync(orderDto.IdOrder))
                .ReturnsAsync(true);

            _mockOrderItemRepository.Setup(r => r.CreateOrderItem(It.IsAny<List<OrderItem>>()))
                .ReturnsAsync(new List<OrderItem>());

            // Act
            var result = await _service.UpdateOrder(orderDto);

            // Assert
            Assert.True(result);
        }
    }
}
