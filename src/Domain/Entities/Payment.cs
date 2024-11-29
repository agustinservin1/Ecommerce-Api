using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    namespace Domain.Entities
    {
        public class Payment
        {
            public string Title { get; set; }
            public int Quantity { get; set; }
            public string CurrencyId { get; set; }
            public decimal UnitPrice { get; set; }
            public string Email { get; set; }
            public string IdentificationType { get; set; }
            public string IdentificationNumber { get; set; }
            public string UrlCheckout { get; set; }
        }
    }
}