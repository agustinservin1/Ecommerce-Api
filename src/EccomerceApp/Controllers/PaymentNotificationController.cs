using Application.Interfaces;
using Application.Models.PaymentModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentNotificationController : ControllerBase
    {
        private readonly IPaymentNotificationService _paymentNotificationService;

        public PaymentNotificationController(IPaymentNotificationService paymentNotificationService)
        {
            _paymentNotificationService = paymentNotificationService;
        }

        [HttpPost]
        [Route("PaymentNotifications")]
        public IActionResult ReceivePaymentNotification([FromBody] InfoPaymentNotification notification)
        {
            if (Request.Headers.TryGetValue("X-Signature", out var xSignature) &&
                Request.Headers.TryGetValue("X-Request-Id", out var xRequestId))
            {
                var dataId = notification.Data.Id;

                if (_paymentNotificationService.ValidateSignature(xSignature, xRequestId, dataId))
                {
                    _paymentNotificationService.ProcessNotification(notification);
                    return Ok();
                }
            }

            return Unauthorized();
        }
    }
}
