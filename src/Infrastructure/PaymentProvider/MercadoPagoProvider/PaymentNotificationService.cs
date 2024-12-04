using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Interfaces;
using MercadoPago.Client.Payment;
using Domain.Entities;
using MercadoPago.Resource.Payment;
using Domain.Enums;
using System.Security.Cryptography;
using System.Text;
using MercadoPago.Config;

public class PaymentNotificationService : IPaymentNotificationService
{
    private readonly IOrderService _orderService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _mercadoPagoSecretKey;
    private readonly string _mercadoPagoTokenKey;

    public PaymentNotificationService(IOrderService orderService, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _orderService = orderService;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _mercadoPagoSecretKey = Environment.GetEnvironmentVariable("MERCADO_PAGO_NOTIFICATION_SECRET") ?? throw new InvalidOperationException("Environment variable 'MERCADO_PAGO_NOTIFICATION_SECRET' not set.");
        _mercadoPagoTokenKey = Environment.GetEnvironmentVariable("MERCADO_PAGO_ACCESS_TOKEN") ?? throw new InvalidOperationException("Environment variable 'MERCADO_PAGO_ACCESS_TOKEN' not set.");
        MercadoPagoConfig.AccessToken = _mercadoPagoTokenKey;


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
    public async Task<bool> ProcessNotification(InfoPaymentNotification notification)
    {
        if (string.IsNullOrWhiteSpace(notification.Data?.Id) || (!long.TryParse(notification.Data.Id, out long paymentId)))
        {
            return false;
        }
        var mercadoPagoPayment = await GetPaymentDetailsAsync(paymentId);
        var result = await ProcessPaymentAndOrder(mercadoPagoPayment);
        await UpdateOrderStatusBasedOnPayment(mercadoPagoPayment);
        return result;
    }
    private async Task<Payment> GetPaymentDetailsAsync(long paymentId)
    {
        var client = new PaymentClient();
        Payment paymentmp = await client.GetAsync(paymentId);
        return paymentmp;
    }
    private async Task<bool> ProcessPaymentAndOrder(Payment mercadoPagoPayment)
    {

        var paymentEntity = MapToPaymentEntity(mercadoPagoPayment);
        await _paymentRepository.Create(paymentEntity);
        return true;
    }
    private async Task<bool> UpdateOrderStatusBasedOnPayment(Payment mercadoPagoPayment)
    {
        if (!int.TryParse(mercadoPagoPayment.ExternalReference, out int orderId))
        {
            throw new InvalidOperationException("Invalid ExternalReference format");
        }

        StatusOrder newStatus;

        switch (mercadoPagoPayment.Status.ToLower())
        {
            case PaymentStatus.Approved:
                newStatus = StatusOrder.Completed;
                break;
            case PaymentStatus.Rejected:
                newStatus = StatusOrder.Canceled;
                break;
            // Maneja otros estados según sea necesario
            default:
                newStatus = StatusOrder.Pending;
                break;
        }

        await _orderService.UpdateOrderStatus(orderId, newStatus);
        return true;
    }

    private Payments MapToPaymentEntity(Payment mercadoPagoPayment)
    {
        if (mercadoPagoPayment == null)
            throw new ArgumentNullException();

        Payments paymentGenerated = new Payments
        {
            PaymentStatus = MapToEnum(mercadoPagoPayment.Status),
            Amount = mercadoPagoPayment.TransactionAmount,
            CreatedAt = mercadoPagoPayment.DateCreated,
            OrderId = int.TryParse(mercadoPagoPayment.ExternalReference, out int orderId) ? orderId : throw new InvalidOperationException("Invalid ExternalReference format"),
            PaymentProviderId = mercadoPagoPayment.Id,
        };

        return paymentGenerated;
    }
    private static PaymentStatusEnum MapToEnum(string status) =>
        status.ToLower() switch
        {
            PaymentStatus.Approved => PaymentStatusEnum.Approved,
            PaymentStatus.Pending => PaymentStatusEnum.Pending,
            PaymentStatus.Authorized => PaymentStatusEnum.Authorized,
            PaymentStatus.InProcess => PaymentStatusEnum.InProcess,
            PaymentStatus.InMediation => PaymentStatusEnum.InMediation,
            PaymentStatus.Rejected => PaymentStatusEnum.Rejected,
            PaymentStatus.Cancelled => PaymentStatusEnum.Cancelled,
            PaymentStatus.Refunded => PaymentStatusEnum.Refunded,
            PaymentStatus.ChargedBack => PaymentStatusEnum.ChargedBack,
            _ => PaymentStatusEnum.Unknown
        };
}
