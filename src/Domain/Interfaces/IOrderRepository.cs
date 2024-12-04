using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<bool> Exists(int orderId);
    }
}
