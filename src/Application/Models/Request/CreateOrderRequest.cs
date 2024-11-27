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
