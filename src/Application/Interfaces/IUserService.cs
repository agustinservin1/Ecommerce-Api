using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task CreateUser(CreateUserRequest request);
        public Task UpdateUser(UpdateUserRequest userRequest);

        public Task DeleteUser(int id);
        public Task<UserDto> GetUserById(int id);
        public Task<IEnumerable<UserDto>> GetAllUsers();
        public Task<IEnumerable<UserDto>> GetUsersByRol(string Role);

    }
}
