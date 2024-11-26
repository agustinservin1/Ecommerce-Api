using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public UserDto? User { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();

        public static OrderDto CreateDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                User = UserDto.CreateDto(order.User),
                TotalAmount = order.TotalPrice,
                OrderDate = order.DateTime,
                Status = order.StatusOrder.ToString(),
                OrderDetails = OrderDetailDto.CreateListDto(order.Details).ToList()
            };
        }

        public static IEnumerable<OrderDto> CreateListDto(IEnumerable<Order> orders)
        {
            return orders.Select(CreateDto).ToList();
        }
    }
}


