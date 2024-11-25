﻿using Application.Interfaces;
using Application.Models.Request;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            if (users.Any())
            {
                throw new NotFoundException("Users", "All");
            }
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            await _userService.CreateUser(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }

        [HttpGet("ByRole/{role}")]
        public async Task<IActionResult> SearchByRole(string role)
        {
            var listUsers = await _userService.GetUsersByRol(role);
            return Ok(listUsers);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            await _userService.UpdateUser(request);
            return NoContent();
        }
    }
}

