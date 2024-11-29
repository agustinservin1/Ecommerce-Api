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
                Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = payment.Title,
                    Quantity = payment.Quantity,
                    CurrencyId = payment.CurrencyId,
                    UnitPrice = payment.UnitPrice
                }
            }
            };

            var client = new PreferenceClient();
            return await client.CreateAsync(preferenceRequest);
        }
    }
}
