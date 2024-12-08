using Domain.Entities;

namespace Application.Models
{

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; } 
        public int ProductDtoId { get; set; } 
        public int Quantity { get; set; }
        public decimal TotalDetail { get; set; }
        public string ProductName { get; set; } 

        public static OrderDetailDto CreateDto(OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.Order.Id,
                ProductDtoId = orderDetail.Product.Id,
                Quantity = orderDetail.Quantity,
                TotalDetail = orderDetail.Total,
                ProductName = orderDetail.Product.Name,
            };
        }

        public static IEnumerable<OrderDetailDto> CreateListDto(IEnumerable<OrderDetail> orderDetails)
        {
            var detailDto = orderDetails.Select(CreateDto).ToList();             

            return detailDto;
        }
    }
}


