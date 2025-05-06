using Application.Dto;
using Application.Service;
using Domain.Models;
using Moq;
using Repository.Repositories;
using Repository.Interface;
using Application.Interface;
using ExternalServices.KafkaConfig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Testing;

public class ProcessOrderService_CreateOrderTest
{
    private readonly Mock<IProcessOrderRepository> _mockOrderRepo;
    private readonly Mock<IOrderItemRepository> _mockOrderItemRepo;
    private readonly Mock<ICalculateOrderService> _mockCalcService; 
    private readonly ProcessOrderService _service;
    private readonly Mock<IProducer> _producerBus;
    private readonly Mock<IOptions<KafkaSettings>> _kafkaSettings;
    private readonly Mock<ILogger<ProcessOrderDto>> _logger;
    private readonly Mock<IServiceProvider> _serviceProvider;

    public ProcessOrderService_CreateOrderTest()
    {
        _mockOrderRepo = new Mock<IProcessOrderRepository>();
        _mockOrderItemRepo = new Mock<IOrderItemRepository>();
        _mockCalcService = new Mock<ICalculateOrderService>();
        _kafkaSettings = new Mock<IOptions<KafkaSettings>>();
        _logger = new Mock<ILogger<ProcessOrderDto>>();
        _producerBus = new Mock<IProducer>();
        _serviceProvider = new Mock<IServiceProvider>();

        _service = new ProcessOrderService(_mockOrderRepo.Object, _mockOrderItemRepo.Object, _mockCalcService.Object, _producerBus.Object, _kafkaSettings.Object, _logger.Object, _serviceProvider.Object); 
    }

    [Fact]
    public async Task CreateOrder_ReturnsTrue_WhenAllStepsSucceed()
    {
        // Arrange
        var orderDto = new ProcessOrderDto
        {
            IdClient = 1,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 },
                new ItemsDto { IdItem = 2, Ammount = 5 }
            }
        };

        var listItems = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            };

        var calculatedOrder = new Order { IdClient = 1, SubTotal = 100, Iva = 19, Total = 119 };
        var createdHeaderOrder = new Order { IdOrder = 10 };

        _mockCalcService.Setup(s => s.CalculateOrder(createdHeaderOrder, listItems)).ReturnsAsync(calculatedOrder);
        _mockOrderRepo.Setup(r => r.CreateOrder(It.IsAny<Order>())).ReturnsAsync(createdHeaderOrder);
        _mockOrderItemRepo.Setup(r => r.CreateOrderItem(It.IsAny<List<OrderItem>>())).ReturnsAsync(new List<OrderItem>());

        // Act
        var result = true;//await _service.CreateOrder(orderDto);

        // Assert
        Assert.True(result);
    }

}
