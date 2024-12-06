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
        Task CreateUser(CreateUserRequest request);
        Task UpdateUser(UpdateUserRequest userRequest);

        Task DeleteUser(int id);
        Task<UserDto> GetUserById(int id);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<IEnumerable<UserDto>> GetUsersByRol(string Role);

    }
}
