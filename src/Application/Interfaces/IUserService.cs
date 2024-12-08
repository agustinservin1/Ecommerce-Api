using Application.Models;
using Application.Models.Request;

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
