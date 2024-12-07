using Application.Interfaces;
using Application.Models.Request;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;


namespace UnitTesting
{
    public class UserServiceTesting
    {
        private readonly UserService _service;
        private readonly Mock<IUserRepository> _mockRepository;

        public UserServiceTesting()
        {
            _mockRepository = new Mock<IUserRepository>();
            _service = new UserService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateUser_Success()
        {
            // Arrange: request de creación
            var createRequest = new CreateUserRequest
            {
                Name = "New User",
                LastName = "Test",
                Email = "newuser@example.com",
                Password = "12345",
                Role = Role.Admin,
            };

            // Act: 
            await _service.CreateUser(createRequest);

            // Assert: corrobora que el método fue llamado correctamente
            _mockRepository.Verify(repo => repo.Create(It.Is<User>(u =>
                u.Name == createRequest.Name &&
                u.LastName == createRequest.LastName &&
                u.Email == createRequest.Email &&
                u.Role == createRequest.Role
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_Success()
        {
            // Arrange: request de actualización y usuario existente
            var updateRequest = new UpdateUserRequest
            {
                Id = 1,
                Name = "Updated User",
                LastName = "Updated LastName",
                Email = "updated@example.com",
                Role = Role.Customer,
            };

            var existingUser = new User
            {
                Id = 1,
                Name = "Old User",
                LastName = "Old LastName",
                Email = "old@example.com",
                Role = Role.Admin,
                Status = Status.Active
            };

            _mockRepository.Setup(repo => repo.GetById(updateRequest.Id)).ReturnsAsync(existingUser);

            // Act
            await _service.UpdateUser(updateRequest);

            // Assert
            _mockRepository.Verify(repo => repo.Update(It.Is<User>(u =>
                u.Id == updateRequest.Id &&
                u.Name == updateRequest.Name &&
                u.LastName == updateRequest.LastName &&
                u.Email == updateRequest.Email &&
                u.Role == updateRequest.Role
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_Success()
        {
            // Arrange: el id del usuario a eliminar y del usuario existente
            var userId = 1;
            var existingUser = new User
            {
                Id = userId,
                Name = "User to Delete",
                LastName = "LASTNAMEUSER",
                Email = "delete@example.com",
                Role = Role.Customer,
                Status = Status.Active
            };

            _mockRepository.Setup(repo => repo.GetById(userId)).ReturnsAsync(existingUser);

            // Act
            await _service.DeleteUser(userId);

            // Assert: verifica que el método llamo al usuario correcto
            _mockRepository.Verify(repo => repo.Delete(It.Is<User>(u => u.Id == userId)), Times.Once);
        }
    }
}
