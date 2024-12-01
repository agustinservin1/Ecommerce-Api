using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.PaymentProvider.MercadoPagoProvider
{
    public class PaymentNotificationService : IPaymentNotificationService
    {
        private readonly string _mercadoPagoSecretKey;
        private readonly IOrderService _orderService;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentNotificationService(IOrderService orderService, IPaymentRepository paymentRepository)
        {
            _mercadoPagoSecretKey = Environment.GetEnvironmentVariable("MERCADO_PAGO_NOTIFICATION_SECRET") ?? throw new InvalidOperationException("Environment variable 'MERCADO_PAGO_NOTIFICATION_SECRET' not set.");
            _orderService = orderService;
            _paymentRepository = paymentRepository;
        }

        public bool ValidateSignature(string xSignature, string xRequestId, string dataId)
        {
            var parts = xSignature.Split(',');
            string? ts = null;
            string? v1 = null;

            foreach (var part in parts)
            {
                var keyValue = part.Split('=', 2);
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    if (key == "ts") { ts = value; }
                    else if (key == "v1") { v1 = value; }
                }
            }

            if (ts == null || v1 == null) return false;

            // Construir el manifest
            var manifest = $"id:{dataId};request-id:{xRequestId};ts:{ts};";

            // Generar el hash HMAC-SHA256
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_mercadoPagoSecretKey)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(manifest));
                var computedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();

                return computedSignature == v1;
            }
        }

        public async Task ProcessNotification(InfoPaymentNotification notification)
        {
            if (int.TryParse(notification.Data.Id, out int orderId))
            {
                var order = await _orderService.GetOrderById(orderId);
                var payment = MapToPaymentEntity(notification, order);
                await _paymentRepository.Create(payment);
                
            }
        }
        private Payment MapToPaymentEntity(InfoPaymentNotification notification, OrderDto order)
        {
            // Validación de los parámetros de entrada
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            if (order == null) throw new ArgumentNullException(nameof(order));

            // Mapeo de la notificación de pago y el pedido a la entidad Payment
            return new Payment
            {
                PaymentId = notification.Data.Id,
                Provider = 1, 
                PaymentStatus = PaymentStatus.Completed, 
                Amount = order.TotalAmount,
                CreatedAt = notification.DateCreated,
                OrderId = order.Id,
                
            };
        }

    }
}
