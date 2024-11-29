using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
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
            public async Task<ProcessPaymentResponse> RedirectMercadopago([FromBody] List<OrderDto> ordersDto)
            {
            {
                var paymentDto = PaymentDto.FromOrderAndDetails(ordersDto);
                var preference = await _paymentService.CreatePaymentAsync(paymentDto);
                return new ProcessPaymentResponse
                {
                    UrlCheckout = preference.SandboxInitPoint }; }
                }
        }
    }

             