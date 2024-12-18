﻿using Domain.Entities;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public static UserDto CreateDto(User user)
        {
            UserDto dto = new();
            {
                dto.Id = user.Id;
                dto.Name = user.Name;
                dto.LastName = user.LastName;
                dto.Email = user.Email;
                dto.Password = user.Password;
                dto.Role = user.Role.ToString();
                dto.Status = user.Status.ToString();
            }
            return dto;
        }

        public static IEnumerable<UserDto> CreateList(IEnumerable<User> users)
        {
            List<UserDto> list = [];
            foreach (User user in users)
            {
                list.Add(CreateDto(user));
            }
            return list;
        }

    }
}
