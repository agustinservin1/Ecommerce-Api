using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
using Infrastructure.PaymentProvider.MercadoPagoProvider;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

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
        public async Task<ProcessPaymentResponse> RedirectMercadopago(int idOrder)
        {

            var preference = await _paymentService.CreatePaymentAsync(idOrder);
            return new ProcessPaymentResponse
            {
                UrlCheckout = preference.SandboxInitPoint
            };

        }
    }
}

