using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }

        public static User ToEntity(CreateUserRequest userDto)
        {
            User user = new User
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = userDto.Role,
            };

            return user;
        }

    }
}
