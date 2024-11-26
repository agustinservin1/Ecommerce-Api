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
        Task<OrderDto> GetOrderById(int id);
        Task<OrderDto> GetAllOrders();
        Task<OrderDto> DeleteOrder(int id);
    }
}
