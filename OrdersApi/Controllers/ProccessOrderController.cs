using Application.Dto;
using Application.Interface;
using Application.Service;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrdersApi.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class ProccessOrderController : ControllerBase
    {
        private readonly IProcessOrderService _processOrderService;

        public ProccessOrderController(IProcessOrderService processOrderService)
        {
            _processOrderService = processOrderService;
        }
               
        [HttpGet]
        [Route("GetOrderById")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId)
        {
            try
            {
                var result = await _processOrderService.GetOrderById(orderId);

                if (result == null)
                    return NotFound($"Pedido nro {orderId}, no encontrado!");
                               
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
           
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(ProcessOrderDto order)
        {
            try
            {
                if (order == null || order.IdClient == 0 || order.items.Count == 0) {
                    return BadRequest(400);
                }

                var result = await _processOrderService.CreateOrder(order);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }

        }

        [HttpPut]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(ProcessOrderDto order)
        {
            try
            {
                if (order == null || order.IdOrder == 0 || order.items.Count == 0)
                {
                    return BadRequest(400);
                }

                var result = await _processOrderService.UpdateOrder(order);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

    }
}
