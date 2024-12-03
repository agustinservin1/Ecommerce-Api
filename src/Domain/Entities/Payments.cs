using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class Payments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Provider { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;   
        public decimal? Amount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int PaymentProviderId { get; set; } // Identificador único del pago en Mercado Pago

}
}
