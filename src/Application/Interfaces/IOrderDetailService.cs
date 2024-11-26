using Application.Models.Request;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

