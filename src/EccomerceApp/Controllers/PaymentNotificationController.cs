using Application.Interfaces;
using Application.Models.PaymentModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("api/[controller]")]
[ApiController]
public class PaymentNotificationController : ControllerBase
{
    private readonly IPaymentNotificationService _paymentNotificationService;
    private readonly ILogger<PaymentNotificationController> _logger;

    public PaymentNotificationController(IPaymentNotificationService paymentNotificationService, ILogger<PaymentNotificationController> logger)
    {
        _paymentNotificationService = paymentNotificationService;
        _logger = logger;
    }

    [HttpPost]
    [Route("PaymentNotifications")]

    public async Task<IActionResult> ReceivePaymentNotification([FromBody] InfoPaymentNotification notification)
    {
        if (Request.Headers.TryGetValue("X-Signature", out var xSignature) &&
            Request.Headers.TryGetValue("X-Request-Id", out var xRequestId))
        {
            var dataId = notification.Data.Id;
            if (_paymentNotificationService.ValidateSignature(xSignature, xRequestId, dataId))
            {
                var result = await _paymentNotificationService.ProcessNotification(notification);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        return Unauthorized();
    }
}