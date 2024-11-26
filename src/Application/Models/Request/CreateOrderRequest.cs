using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreateOrderRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public decimal TotalAcount { get; set; }
        [Required]
        public List<CreateOrderDetailRequest> OrderDetails { get; set; } = new List<CreateOrderDetailRequest>();
        public static Order ToEntity(CreateOrderRequest dto, User user, List<OrderDetail> orderDetails)
        {
            return new Order
            {
                User = user,
                DateTime = DateTime.UtcNow,
                StatusOrder = Enum.TryParse(dto.Status, true, out StatusOrder status) ? status : StatusOrder.Pending, //true insensible a mayusc - out = valor de salida, 
                TotalPrice = orderDetails.Sum(detail => detail.Total),
                Details = orderDetails
            };
        }

    }
}
