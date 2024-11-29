using Application.Interfaces;
using Application.Models.PaymentModels;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PaymentProvider.MercadoPagoProvider
{

    public class PaymentService : IPaymentService
    {
        public PaymentService()
        {
            MercadoPagoConfig.AccessToken = Environment.GetEnvironmentVariable("MERCADO_PAGO_ACCESS_TOKEN");
        }

        public async Task<Preference> CreatePaymentAsync(PaymentDto payment)
        {
            var preferenceRequest = new PreferenceRequest
            {
                Items = payment.Items
            };

            var client = new PreferenceClient();
            return await client.CreateAsync(preferenceRequest);
        }
    }
}



