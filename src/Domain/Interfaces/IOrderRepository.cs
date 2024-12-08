using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<bool> Exists(int orderId);
    }
}
