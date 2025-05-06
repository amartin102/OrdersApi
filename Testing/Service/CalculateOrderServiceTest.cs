using Application.Dto;
using Application.Service;
using Domain.Models;
using Moq;
using Repository.Interface;

namespace Testing;

public class CalculateOrderServiceTest
{
    private readonly Mock<IItemRepository> _mockItemRepo;
    private readonly CalculateOrderService _service;

    public CalculateOrderServiceTest()
    {
        _mockItemRepo = new Mock<IItemRepository>();
        _service = new CalculateOrderService(_mockItemRepo.Object);
    }

    [Fact]
    public async Task CalculateOrder_ReturnsOrderWithCorrectTotals()
    {
        // Arrange
        var itemsFromDb = new List<Item>
        {
            new Item { IdItem = 1, PriceUnit = 100 },
            new Item { IdItem = 2, PriceUnit = 50 }
        };

        var inputOrder = new Order();
        var orderItems = new List<ItemsDto>
        {
            new ItemsDto { IdItem = 1, Ammount = 2 }, // 2 * 100 = 200
            new ItemsDto { IdItem = 2, Ammount = 1 }  // 1 * 50 = 50
        };

        _mockItemRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(itemsFromDb);

        // Act
        var result = await _service.CalculateOrder(inputOrder, orderItems);

        // Assert
        Assert.Equal(250, result.SubTotal);
        Assert.Equal(47.5m, result.Iva); // 19% de 250
        Assert.Equal(297.5m, result.Total);
    }
       
}
