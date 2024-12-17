using Application.Interfaces;
using Application.Models.PaymentModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentNotificationController : ControllerBase
    {
        private readonly IPaymentNotificationService _paymentNotificationService;

        public PaymentNotificationController(IPaymentNotificationService paymentNotificationService)
        {
            _paymentNotificationService = paymentNotificationService;
        }

        [HttpPost]
        [Route("PaymentNotifications")]
        public async Task<IActionResult> ReceivePaymentNotification([FromBody] InfoPaymentNotification notification)
        {
            return Request.Headers.TryGetValue("X-Signature", out var xSignature) &&
                   Request.Headers.TryGetValue("X-Request-Id", out var xRequestId) &&
                   !string.IsNullOrWhiteSpace(notification.Data?.Id) &&
                   _paymentNotificationService.ValidateSignature(xSignature.ToString(), xRequestId.ToString(), notification.Data!.Id)
                ? await _paymentNotificationService.ProcessNotification(notification)
                    ? Ok()
                    : BadRequest()
                : Unauthorized();
        }
    }
}