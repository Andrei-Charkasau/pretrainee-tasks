using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using InnoShop.Auth.Services.Services;
using InnoShop.Auth.Controllers;
using InnoShop.Shared.Domain.Models;
using InnoShop.Auth.Services.DtoModels;

namespace InnoShop.Auth.Tests.UnitTests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _usersController;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _usersController = new UsersController(_mockUserService.Object);
        }

        private void SetupUserContext(int userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _usersController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task GetAllUsersAsync_WhenAdmin_ReturnsAllUsers()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var users = new List<User>
            {
                new User { Id = 1, Name = "User 1", Email = "user1@example.com" },
                new User { Id = 2, Name = "User 2", Email = "user2@example.com" }
            };

            _mockUserService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _usersController.GetAllUsersAsync();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetUserAsync_WhenAdmin_ReturnsUser()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var userId = 2;
            var user = new User { Id = userId, Name = "Test User", Email = "test@example.com" };

            _mockUserService.Setup(service => service.GetAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _usersController.GetUserAsync(userId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetUserAsync_WhenUserNotFound_ReturnsOkWithNull()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var userId = 999;

            _mockUserService.Setup(service => service.GetAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _usersController.GetUserAsync(userId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_WhenAdmin_ReturnsOk()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var registerDto = new RegisterDto
            {
                Name = "New User",
                Email = "new@example.com",
                Password = "password123",
                Role = "User"
            };

            _mockUserService.Setup(service => service.CreateAsync(registerDto))
                .ReturnsAsync(new User { Id = 1, Email = "new@example.com" });

            // Act
            var result = await _usersController.CreateUserAsync(registerDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("User was registered successfully. [+]");
            _mockUserService.Verify(service => service.CreateAsync(registerDto), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_WhenRegularUser_ReturnsOk()
        {
            // Arrange
            SetupUserContext(1, "User");

            var registerDto = new RegisterDto
            {
                Name = "New User",
                Email = "new@example.com",
                Password = "password123",
                Role = "User"
            };

            _mockUserService.Setup(service => service.CreateAsync(registerDto))
                .ReturnsAsync(new User { Id = 2, Email = "new@example.com" });

            // Act
            var result = await _usersController.CreateUserAsync(registerDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateUserAsync_WithDuplicateEmail_ThrowsException()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var registerDto = new RegisterDto
            {
                Name = "Existing User",
                Email = "existing@example.com",
                Password = "password123",
                Role = "User"
            };

            _mockUserService.Setup(service => service.CreateAsync(registerDto))
                .ThrowsAsync(new Exception("User with this email already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _usersController.CreateUserAsync(registerDto));
        }

        [Fact]
        public async Task UpdateUserAsync_WhenAdmin_ReturnsNoContent()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var userId = 2;
            var userDto = new UserDto
            {
                Name = "Updated User",
                Email = "updated@example.com",
                Role = "User"
            };

            _mockUserService.Setup(service => service.UpdateAsync(userId, userDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _usersController.UpdateUserAsync(userId, userDto);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
            _mockUserService.Verify(service => service.UpdateAsync(userId, userDto), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var userId = 999;
            var userDto = new UserDto
            {
                Name = "Updated User",
                Email = "updated@example.com",
                Role = "User"
            };

            _mockUserService.Setup(service => service.UpdateAsync(userId, userDto))
                .ThrowsAsync(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _usersController.UpdateUserAsync(userId, userDto));
        }

        [Fact]
        public async Task DeleteUserAsync_WhenAdmin_ReturnsNoContent()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var userId = 2;

            _mockUserService.Setup(service => service.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _usersController.DeleteUserAsync(userId);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
            _mockUserService.Verify(service => service.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserNotFound_ThrowsException()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var userId = 999;

            _mockUserService.Setup(service => service.DeleteAsync(userId))
                .ThrowsAsync(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _usersController.DeleteUserAsync(userId));
        }

        [Fact]
        public async Task CreateUserAsync_WithInvalidData_ThrowsException()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var registerDto = new RegisterDto
            {
                Name = "",
                Email = "invalid-email",
                Password = "123",
                Role = "InvalidRole"
            };

            _mockUserService.Setup(service => service.CreateAsync(registerDto))
                .ThrowsAsync(new ArgumentException("Invalid user data"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _usersController.CreateUserAsync(registerDto));
        }
    }
}