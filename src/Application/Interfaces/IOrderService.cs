using Application.Models.Request;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
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
