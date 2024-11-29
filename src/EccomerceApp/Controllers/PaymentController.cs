using Application.Interfaces;
using Application.Models.PaymentModels;
using Infrastructure.PaymentProvider.MercadoPagoProvider;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
  
        [EnableCors("CorsPolicy")]
        [ApiController]
        [Route("/")]
        public class MercadopagoController : ControllerBase
        {
            private readonly IPaymentService _paymentService;

            public MercadopagoController(IPaymentService paymentService)
            {
                _paymentService = paymentService;
            }

            [HttpPost]
            [Route("redirect-mercadopago")]
            public async Task<ProcessPaymentResponse> RedirectMercadopago([FromBody] PaymentDto paymentDto)
            {
                var preference = await _paymentService.CreatePaymentAsync(paymentDto);
                return new ProcessPaymentResponse
                {
                    UrlCheckout = preference.SandboxInitPoint
                };
            }
        }
    }

  