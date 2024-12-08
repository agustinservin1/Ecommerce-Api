using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Product Product { get; set; } =new Product();

        public Order Order { get; set; } = new Order();
        public int Quantity { get; set; }
        public decimal Total {  get; set; } 
    }
}
