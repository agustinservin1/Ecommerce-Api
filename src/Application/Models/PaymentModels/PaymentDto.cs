using Domain.Entities;
using Domain.Entities.Domain.Entities;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.PaymentModels
{
    public class PaymentDto
    {
        public string CurrencyId { get; set; } = "ARS"; 
        public List<PreferenceItemRequest> Items { get; set; } = new List<PreferenceItemRequest>(); 

        public static PaymentDto FromOrderAndDetails(List<OrderDto> orders)
        {
            var paymentDto = new PaymentDto();

            foreach (var order in orders)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    paymentDto.Items.Add(new PreferenceItemRequest
                    {
                        Title = orderDetail.ProductName, 
                        Quantity = orderDetail.Quantity,
                        CurrencyId = "ARS",
                        UnitPrice = orderDetail.TotalDetail/orderDetail.Quantity
                    });
                }
            }

            return paymentDto;
        }
    }

}

    


