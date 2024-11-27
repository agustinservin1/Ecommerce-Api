using Application.Models.Request;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderRequest orderRequest);
        Task<OrderDto> ConfirmOrder(int orderId);
        Task<OrderDto> CancelOrder(int orderId);
        Task<OrderDto> GetOrderById(int id);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> DeleteOrder(int id);
    }
}
