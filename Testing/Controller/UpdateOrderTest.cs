using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersApi.Controllers;

namespace Testing;

public class UpdateOrderTest
{
    private readonly Mock<IProcessOrderService> _mockService;
    private readonly ProccessOrderController _controller;

    public UpdateOrderTest()
    {
        _mockService = new Mock<IProcessOrderService>();
        _controller = new ProccessOrderController(_mockService.Object);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsOkTrue_WhenUpdateSuccessful()
    {
        // Arrange
        var order = new ProcessOrderDto
        {
            IdOrder = 10,
            IdClient = 1,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            }
        };

        _mockService.Setup(s => s.UpdateOrder(order)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<bool>(okResult.Value);
        Assert.True(returnValue);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsOkFalse_WhenUpdateFails()
    {
        // Arrange
        var order = new ProcessOrderDto
        {
            IdOrder = 10,
            IdClient = 1,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            }
        };

        _mockService.Setup(s => s.UpdateOrder(order)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<bool>(okResult.Value);
        Assert.False(returnValue);
    }

    [Theory]
    [InlineData(0)] // IdOrder inválido
    public async Task UpdateOrder_ReturnsBadRequest_WhenOrderIsInvalid(int orderId)
    {
        // Arrange
        var invalidOrder = new ProcessOrderDto
        {
            IdOrder = orderId,
            IdClient = 1,
            items = new List<ItemsDto>() // vacío
        };

        // Act
        var result = await _controller.UpdateOrder(invalidOrder);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var order = new ProcessOrderDto
        {
            IdOrder = 5,
            IdClient = 1,
            items = new List<ItemsDto>
            {
                new ItemsDto { IdItem = 1, Ammount = 2 }
            }
        };

        _mockService.Setup(s => s.UpdateOrder(order))
                    .ThrowsAsync(new System.Exception("DB error"));

        // Act
        var result = await _controller.UpdateOrder(order);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.IsType<System.Exception>(statusResult.Value);
    }
}
