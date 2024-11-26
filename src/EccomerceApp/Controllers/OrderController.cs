using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService) {
            _orderService = orderService;
        }
        [HttpGet("/GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            return Ok(order);
        }
        [HttpGet("/GetAllOrders")]
        public async Task<IActionResult> GetAll()
        { 
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }
        [HttpPost("/CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderRequest createOrder)
        {
            var order = await _orderService.CreateOrder(createOrder);
            return NoContent();
        }
        [HttpDelete("/DeleteOrder{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleteOrder = await _orderService.DeleteOrder(id);
            return Ok(deleteOrder);
        }
    }
}
