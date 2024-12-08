using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class Payments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Provider { get; set; } = 1;
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;   
        public decimal? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public long? PaymentProviderId { get; set; } // Identificador único del pago en Mercado Pago

}
}
