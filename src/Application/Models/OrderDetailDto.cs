using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models
{

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderDtoId { get; set; } 
        public int ProductDtoId { get; set; } 
        public int Quantity { get; set; }
        public decimal TotalDetail { get; set; }


        public static OrderDetailDto CreateDto(OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                OrderDtoId = orderDetail.Order.Id,
                ProductDtoId = orderDetail.Product.Id,
                Quantity = orderDetail.Quantity,
                TotalDetail = orderDetail.Total,
            };
        }

        public static IEnumerable<OrderDetailDto> CreateListDto(IEnumerable<OrderDetail> orderDetails)
        {
            var detailDto = orderDetails.Select(CreateDto).ToList();             

            return detailDto;
        }
    }
}


