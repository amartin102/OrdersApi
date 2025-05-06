using Application.Dto;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrdersApi.Controllers;

namespace Testing;

public class GetOrderByIdTest
{
    private readonly Mock<IProcessOrderService> _mockService;
    private readonly ProccessOrderController _controller;

    public GetOrderByIdTest()
    {
        _mockService = new Mock<IProcessOrderService>();
        _controller = new ProccessOrderController(_mockService.Object);
    }

    [Fact]
    public async Task GetOrderById_ReturnsOk_WhenOrderExists()
    {
        // Arrange
        int orderId = 1;
        var expectedOrder = new OrderDto { IdOrder = orderId, ClientName  = "Laura Gómez", Address = "Calle 44 5 - 22 San juan - Medellín", 
            SubTotal = "1250.000", Iva = "237.500", Total = 1487, Status = 1 };

        _mockService.Setup(service => service.GetOrderById(orderId))
                    .ReturnsAsync(expectedOrder);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<OrderDto>(okResult.Value);
        Assert.Equal(orderId, returnValue.IdOrder);
    }

    [Fact]
    public async Task GetOrderById_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int orderId = 999;

        _mockService.Setup(service => service.GetOrderById(orderId))
                    .ReturnsAsync((OrderDto)null);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains($"Pedido nro {orderId}, no encontrado!", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task GetOrderById_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        int orderId = 2;
        _mockService.Setup(service => service.GetOrderById(orderId))
                    .ThrowsAsync(new System.Exception("DB connection error"));

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Contains("Error interno", statusResult.Value.ToString());
    }
}
