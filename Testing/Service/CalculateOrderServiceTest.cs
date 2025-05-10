using Application.Dto;
using Application.Service;
using Domain.Models;
using Moq;
using Repository.Interface;
using Xunit;

namespace Testing.Service
{
    public class CalculateOrderServiceTest
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly CalculateOrderService _calculateOrderService;

        public CalculateOrderServiceTest()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _calculateOrderService = new CalculateOrderService(_mockItemRepository.Object);
        }

        [Fact]
        public async Task CalculateOrder_ReturnsCorrectOrder_WhenItemsAreValid()
        {
            // Arrange
            var order = new Order();
            var orderItems = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 },
                new ItemsDto { IdItem = 2, Ammount = 1 }
            };

            var items = new List<Item>
            {
                new Item { IdItem = 1, PriceUnit = 100 },
                new Item { IdItem = 2, PriceUnit = 200 }
            };

            _mockItemRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _calculateOrderService.CalculateOrder(order, orderItems);

            // Assert
            Assert.Equal(400, result.SubTotal); // 2*100 + 1*200
            Assert.Equal(76, result.Iva); // 19% de 400
            Assert.Equal(476, result.Total); // SubTotal + Iva
        }

        [Fact]
        public async Task CalculateOrder_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            var order = new Order();
            var orderItems = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            };

            _mockItemRepository.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _calculateOrderService.CalculateOrder(order, orderItems));
        }

        [Fact]
        public async Task CalculateOrder_ReturnsZeroValues_WhenOrderItemsIsEmpty()
        {
            // Arrange
            var order = new Order();
            var orderItems = new List<ItemsDto>(); // Lista vacía

            var items = new List<Item>
            {
                new Item { IdItem = 1, PriceUnit = 100 },
                new Item { IdItem = 2, PriceUnit = 200 }
            };

            _mockItemRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            // Act
            var result = await _calculateOrderService.CalculateOrder(order, orderItems);

            // Assert
            Assert.Equal(0, result.SubTotal);
            Assert.Equal(0, result.Iva);
            Assert.Equal(0, result.Total);
        }
    }
}
