using Application.Models;
using Application.Models.Request;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderRequest orderRequest);
        Task<OrderDto> UpdateOrderStatus(int orderId, StatusOrder newStatus);
        Task<OrderDto> GetOrderById(int id);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> DeleteOrder(int id);
    }
}
