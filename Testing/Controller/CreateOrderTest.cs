using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersApi.Controllers;

namespace Testing;

public class CreateOrderTest
{
    private readonly Mock<IProcessOrderService> _mockService;
    private readonly ProccessOrderController _controller;

    public CreateOrderTest()
    {
        _mockService = new Mock<IProcessOrderService>();
        _controller = new ProccessOrderController(_mockService.Object);
    }

    [Fact]
    public async Task CreateOrder_ReturnsOkTrue_WhenOrderIsValid()
    {
        // Arrange
        var order = new ProcessOrderDto
        {
            IdClient = 1,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            }
        };

        _mockService.Setup(s => s.CreateOrder(order)).ReturnsAsync(true);

        // Act
        var result = await _controller.CreateOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<bool>(okResult.Value);
        Assert.True(returnValue);
    }

    [Fact]
    public async Task CreateOrder_ReturnsOkFalse_WhenOrderIsNotCreated()
    {
        // Arrange
        var order = new ProcessOrderDto
        {
            IdClient = 0,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            }
        };

        _mockService.Setup(s => s.CreateOrder(order)).ReturnsAsync(false);

        // Act
        var result = await _controller.CreateOrder(order);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    public async Task CreateOrder_ReturnsBadRequest_WhenOrderIsInvalid(int clientId)
    {
        // Arrange
        var invalidOrder = new ProcessOrderDto
        {
            IdClient = clientId,
            items = new List<ItemsDto>() // vacía
        };

        // Act
        var result = await _controller.CreateOrder(invalidOrder);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var order = new ProcessOrderDto
        {
            IdClient = 1,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            }
        };

        _mockService.Setup(s => s.CreateOrder(order))
                    .ThrowsAsync(new System.Exception("Unexpected error"));

        // Act
        var result = await _controller.CreateOrder(order);

        // Assert
        var errorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, errorResult.StatusCode);
        Assert.Contains("Error interno", errorResult.Value.ToString());
    }
}
