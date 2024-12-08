using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User User { get; set; } = new User();
        public decimal TotalPrice { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public StatusOrder StatusOrder { get; set; } = StatusOrder.Pending;
        public List<OrderDetail> Details { get; set; } = new List<OrderDetail>();
        public Payments Payment { get; set; }

    }
}
