using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Logging;

namespace Infrastructure.PaymentProvider.MercadoPagoProvider
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IOrderService orderService, IPaymentRepository paymentRepository)
        {
            var accessToken = Environment.GetEnvironmentVariable("MERCADO_PAGO_ACCESS_TOKEN");
            MercadoPagoConfig.AccessToken = accessToken;
            _orderService = orderService;
            _paymentRepository = paymentRepository;
        }

        public async Task<Preference> CreatePaymentAsync(int idOrder)
        {

            var order = await _orderService.GetOrderById(idOrder);
            if (order == null)
            {
                throw new NotFoundException($"The order with id {idOrder} does not exist.");
            }
            var preferenceRequest = GeneratePreferenceRequest(order);
            var client = new PreferenceClient();
            return await client.CreateAsync(preferenceRequest); ;
        }
        private PreferenceRequest GeneratePreferenceRequest(OrderDto order)
        {
            return new PreferenceRequest
            {
                Items = order?.OrderDetails?.Select(detail => new PreferenceItemRequest
                {
                    Title = detail.ProductName,
                    Quantity = detail.Quantity,
                    CurrencyId = "ARS",
                    UnitPrice = detail.TotalDetail / detail.Quantity
                }).ToList(),

                NotificationUrl = "https://7c14-149-102-233-167.ngrok-free.app/api/PaymentNotification/PaymentNotifications",
                ExternalReference = order?.Id.ToString(),
                Payer = new PreferencePayerRequest
                {
                    Name = order?.User?.Name,
                    Surname = order?.User?.LastName,
                    Email = order?.User?.Email
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "http://httpbin.org/get?back_url=success",
                    Failure = "http://httpbin.org/get?back_url=failure",
                    Pending = "http://httpbin.org/get?back_url=pending"
                },
                AutoReturn = "approved",
                Expires = true,
                ExpirationDateFrom = DateTime.UtcNow,
                ExpirationDateTo = DateTime.UtcNow.AddDays(10)
            };
        }
        public async Task<IEnumerable<Payments>> GetAllPayments()
        {
            return await _paymentRepository.GetAllPaymentsRepository();

        }

    }
}
