using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Controllers;
using Application.Models;
using Application.Models.Request;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class UserControllerTesting
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _mockService;

        public UserControllerTesting()
        {
            //Instanciación del mock
            //Simula la lógica detrás de la interfaz
            //- Parametros :
            // - setup (permite identificar que metodos vamos a simular,configurar un parametro de entrada y definir el return)
            // - object (dentro de nuestra variable mock nos permite acceder al tipo que estamos imitando)
            // - verify (comprobar que las llamadas simuladas se van a ejecutar)
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

        [Fact]
        public async Task GetById_OkResult()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto
            {
                Id = userId,
                Name = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "password123",
                Role = "Customer",
                Status = "Active"
            };

            _mockService.Setup(service => service.GetUserById(userId)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnValue.Id);
            Assert.Equal("John", returnValue.Name);

            // Verify
            _mockService.Verify(service => service.GetUserById(userId), Times.Once);
        }

        [Fact]
        public async Task GetAll_OkResult()
        {
            // Arrange
            var userList = new List<UserDto>
            {
                new UserDto { Id = 1, Name = "John", LastName = "Doe", Email = "john@example.com", Password = "password123", Role = "Customer", Status = "Active" },
                new UserDto { Id = 2, Name = "Jane", LastName = "Doe", Email = "jane@example.com", Password = "password123", Role = "Customer", Status = "Active" }
            };
            _mockService.Setup(service => service.GetAllUsers()).ReturnsAsync(userList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<UserDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);

            // Verify
            _mockService.Verify(service => service.GetAllUsers(), Times.Once);
        }

        [Fact]
        public async Task CreateUser_NoContentResult()
        {
            // Arrange
            var createRequest = new CreateUserRequest
            {
                Name = "New User",
                LastName = "Test",
                Email = "newuser@example.com",
                Password = "securepassword",
                Role = Domain.Enums.Role.Admin,
            };

            _mockService.Setup(service => service.CreateUser(createRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateUser(createRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify
            _mockService.Verify(service => service.CreateUser(createRequest), Times.Once);
        }

    }
}
