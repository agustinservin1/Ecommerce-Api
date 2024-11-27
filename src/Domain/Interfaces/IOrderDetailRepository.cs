using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetail>
    {
        Task<OrderDetail> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task CreateOrderDetailsAsync(List<OrderDetail> orderDetails);
        Task<List<OrderDetail>> GetAllOrderDetailsAsync();
    }
}
