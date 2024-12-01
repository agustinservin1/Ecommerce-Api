using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class Payment
    {
        public string PaymentId { get; set; }
        public int Provider { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;   
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
