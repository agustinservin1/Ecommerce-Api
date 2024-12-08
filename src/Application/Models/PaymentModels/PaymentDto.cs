using MercadoPago.Client.Preference;

namespace Application.Models.PaymentModels
{
    public class PaymentDto
    {
        public string CurrencyId { get; set; } = "ARS"; 
        public List<PreferenceItemRequest> Items { get; set; } = new List<PreferenceItemRequest>(); 

        
    }

}

    


