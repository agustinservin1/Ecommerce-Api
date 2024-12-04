using Application.Interfaces;
using Application.Models;
using Application.Models.PaymentModels;
using Domain.Entities;
using Infrastructure.PaymentProvider.MercadoPagoProvider;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet]
        [Route("/GetPayments")]
        public async Task <IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            
            return Ok(payments);
        }
        [HttpGet]
        [Route("/GetPaymentById(id)")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            return Ok(payment);
        }
    }
}

