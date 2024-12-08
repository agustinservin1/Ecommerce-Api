using Application.Models;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface IOrderDetailService
    {

        Task<IEnumerable<OrderDetailDto>> GetAllOrderDetails();
        Task<OrderDetailDto> GetOrderDetailById(int id);
        Task<OrderDetailDto> CreateOrderDetail(CreateOrderDetailRequest request, int orderId);
        //Task<OrderDetailDto> UpdateOrderDetail(int id, UpdateOrderDetailRequest request);
        Task<bool> DeleteOrderDetail(int id);
    }
}

