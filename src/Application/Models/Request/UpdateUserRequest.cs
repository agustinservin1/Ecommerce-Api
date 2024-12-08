using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
       
        [Required]
        public Role Role { get; set; }
    }
}
