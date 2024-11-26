using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public OrderDto Order { get; set; } = new OrderDto();
        public ProductDto Product { get; set; } = new ProductDto(); 
        public int Quantity { get; set; }
        public decimal Total { get; set; }


        public static OrderDetailDto CreateDto(OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                Order=OrderDto.CreateDto(orderDetail.Order),
                Product = ProductDto.CreateDto(orderDetail.Product), 
                Quantity = orderDetail.Quantity,
                Total = orderDetail.Total,
            };
        }

        public static IEnumerable<OrderDetailDto> CreateListDto(IEnumerable<OrderDetail> orderDetails)
        {
            return orderDetails.Select(CreateDto).ToList();
        }
    }
}


