using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public static UserDto CreateDto(User user)
        {
            UserDto dto = new UserDto();
            {
                dto.Id = user.Id;
                dto.FullName = user.FullName;
                dto.Email = user.Email;
                dto.Password = user.Password;
                dto.Role = user.Role.ToString();
                dto.Status = user.Status.ToString();
            }
            return dto;
        }

        public static IEnumerable<UserDto> CreateList(IEnumerable<User> users)
        {
            List<UserDto> list = new List<UserDto>();
            foreach (User user in users)
            {
                list.Add(CreateDto(user));
            }
            return list;
        }

    }
}
