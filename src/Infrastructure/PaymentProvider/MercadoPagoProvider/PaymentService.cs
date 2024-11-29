using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
using Domain.Exceptions;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PaymentProvider.MercadoPagoProvider
{

    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        public PaymentService(IOrderService orderService)
        {
            MercadoPagoConfig.AccessToken = Environment.GetEnvironmentVariable("MERCADO_PAGO_ACCESS_TOKEN");
            _orderService = orderService;
        }

        public async Task<Preference> CreatePaymentAsync(int idOrder)
        {
            OrderDto order = await _orderService.GetOrderById(idOrder);
            if (order == null)
            {
                throw new NotFoundException($"The order with id {idOrder} does not exist.");
            }
            var paymentDto = FromOrderAndDetails(order); 
            var preferenceRequest = new PreferenceRequest
            {
                Items = paymentDto.Items
            };
            var client = new PreferenceClient();
            return await client.CreateAsync(preferenceRequest);
            }
        private PaymentDto FromOrderAndDetails(OrderDto order)
        {
            var paymentDto = new PaymentDto();
            foreach (var orderDetail in order.OrderDetails) 
            { paymentDto.Items.Add(new PreferenceItemRequest
            { Title = orderDetail.ProductName,
              Quantity = orderDetail.Quantity, CurrencyId = "ARS",
              UnitPrice = orderDetail.TotalDetail / orderDetail.Quantity
            });
            } 
            return paymentDto;
        }
    }
}



