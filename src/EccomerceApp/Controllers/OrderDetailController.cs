using Application.Interfaces;
using Application.Models.Request;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService) {
            _orderDetailService = orderDetailService;
        }
        [HttpGet("GetById({id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderDetailService.GetOrderDetailById(id);
            return Ok(order);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderDetailService.GetAllOrderDetails();
            return Ok(orders);
        }
        [HttpPost("{id}/details")]
        public async Task<IActionResult> CreateOrderDetail([FromBody] CreateOrderDetailRequest createOrder, int id)
        {
            try
            {
                var orderDetail = await _orderDetailService.CreateOrderDetail(createOrder, id);
                return CreatedAtAction(nameof(CreateOrderDetail), new { id = orderDetail.Id }, orderDetail);
            }
            catch (Exception ex) // Captura cualquier excepción
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("DeleteOrderDetail{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var deleteOrderDetail = await _orderDetailService.DeleteOrderDetail(id);
            return Ok(deleteOrderDetail);
        }


    }
}
