using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task CreateUser(CreateUserRequest userRequest)
        {
            if (userRequest == null) throw new NotFoundException("Request is null");
            if (!Enum.IsDefined(typeof(Role), userRequest.Role))
            {
                throw new NotFoundException("Invalid role");
            }
            var user = CreateUserRequest.ToEntity(userRequest);
            await _repository.Create(user);
        }
        public async Task DeleteUser(int id)
        {

            var user = await _repository.GetById(id);

            if (user == null)
            {
                throw new NotFoundException($"The user with id {id} does not exist");
            }

            await _repository.Delete(user);
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null)
            {
                throw new NotFoundException($"The user with id {id} does not exist");
            }
            return UserDto.CreateDto(user);
        }
        public async Task<IEnumerable<UserDto>>? GetAllUsers()
        {
            var listUser = await _repository.GetAll();
            return UserDto.CreateList(listUser);
        }

    }
}
