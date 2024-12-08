using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; }   = string.Empty ;
        public string Password {  get; set; } = string.Empty ;
        public Role Role { get; set; }
        public Status Status { get; set; }

        public List<Order> OrdersList { get; set; } = new List<Order>();


    }
}
