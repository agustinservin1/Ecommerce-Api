using Domain.Entities;

namespace Application.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; } = new UserDto();
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        public List<OrderDetailDto>? OrderDetails { get; set; } 

        public static OrderDto CreateDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                User = UserDto.CreateDto(order.User),
                TotalAmount = order.TotalPrice,
                OrderDate = order.DateTime,
                Status = order.StatusOrder.ToString(),
                OrderDetails = order.Details.Select(OrderDetailDto.CreateDto).ToList(), 
            };
        }

        public static IEnumerable<OrderDto> CreateListDto(IEnumerable<Order> orders)
        {
            List<OrderDto> list = new List<OrderDto>();
            foreach (Order order in orders)
            {
                list.Add(CreateDto(order));
            }
            return list;

        }
    }
}



