using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateOrderRequest
    {
            [Required]
            public int UserId { get; set; }
            public static Order ToEntity(CreateOrderRequest dto, User user)
            {
                return new Order
                {
                    User = user,
                    DateTime = DateTime.UtcNow,
                    StatusOrder = StatusOrder.Pending,
                    TotalPrice = 0, 
                    Details = new List<OrderDetail>() 
                };
            }
        }
    }
