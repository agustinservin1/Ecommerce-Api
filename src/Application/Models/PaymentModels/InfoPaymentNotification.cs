using System;

namespace Application.Models.PaymentModels
{
    public class InfoPaymentNotification
    {
        public string Action { get; set; }
        public string ApiVersion { get; set; } = "v1";// Versión de la API REVISAR TIPO DE DATO.
        public Data Data { get; set; } 
        public string Id { get; set; } 
        public bool LiveMode { get; set; }
        public string Type { get; set; } 
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; } = "1"; // REVISAR
    }

    public class Data
    {
        public string Id { get; set; } // ID del recurso relacionado (por ejemplo, un pago) como string.
    }
}
