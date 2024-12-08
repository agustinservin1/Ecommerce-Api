using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateOrderDetailRequest
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public static OrderDetail ToEntity(CreateOrderDetailRequest dto, Product product, Order order)
        {
            return new OrderDetail
            {
                Order = order,       
                Product = product,    
                Quantity = dto.Quantity,
                Total = product.Price * dto.Quantity 
            };
        }
    }
}
