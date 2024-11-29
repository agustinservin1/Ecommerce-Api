using Domain.Entities;
using Domain.Entities.Domain.Entities;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.PaymentModels
{
    public class PaymentDto
    {
        public string CurrencyId { get; set; } = "ARS"; 
        public List<PreferenceItemRequest> Items { get; set; } = new List<PreferenceItemRequest>(); 

        
    }

}

    


