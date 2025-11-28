using FluentAssertions;
using InnoShop.Auth.Services.DtoModels;
using InnoShop.Auth.Services.Services;
using InnoShop.Core.Services.Services;
using InnoShop.Shared.Domain.Models;
using InnoShop.Shared.Infrastructure.Exceptions;
using InnoShop.Shared.Infrastructure.Repositories;
using MockQueryable;
using Moq;

namespace InnoShop.Auth.Tests.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User, int>> _mockUserRepository;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Mock<IProductService> _mockProductService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User, int>>();
            _mockJwtService = new Mock<IJwtService>();
            _mockProductService = new Mock<IProductService>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockJwtService.Object,
                _mockProductService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password123" };
            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                IsEmailConfirmed = true
            };
            var expectedToken = "jwt_token";

            var users = new List<User> { user }.BuildMock();

            _mockUserRepository.Setup(repo => repo.GetAll())
                .Returns(users);
            _mockJwtService.Setup(jwt => jwt.VerifyPassword("password123", "hashed_password"))
                .Returns(true);
            _mockJwtService.Setup(jwt => jwt.GenerateToken(user))
                .Returns(expectedToken);

            // Act
            var result = await _userService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().Be("Bearer " + expectedToken);
        }

        [Fact]
        public async Task AuthenticateAsync_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "wrong@example.com", Password = "password123" };

            var users = new List<User>().BuildMock();

            _mockUserRepository.Setup(repo => repo.GetAll())
                .Returns(users);

            // Act
            var result = await _userService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_WithUnconfirmedEmail_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password123" };
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                IsEmailConfirmed = false
            };

            var users = new List<User> { user }.BuildMock();

            _mockUserRepository.Setup(repo => repo.GetAll())
                .Returns(users);
            _mockJwtService.Setup(jwt => jwt.VerifyPassword("password123", "hashed_password"))
                .Returns(true);

            // Act
            var result = await _userService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_WithWrongPassword_ReturnsNull()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "wrong_password" };
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = "hashed_password",
                IsEmailConfirmed = true
            };

            var users = new List<User> { user }.BuildMock();

            _mockUserRepository.Setup(repo => repo.GetAll())
                .Returns(users);
            _mockJwtService.Setup(jwt => jwt.VerifyPassword("wrong_password", "hashed_password"))
                .Returns(false);

            // Act
            var result = await _userService.AuthenticateAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_WithDuplicateEmail_ThrowsBusinessException()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "existing@example.com",
                Password = "password123",
                Role = "User"
            };

            var existingUser = new User { Email = "existing@example.com" };
            var users = new List<User> { existingUser }.BuildMock();

            _mockUserRepository.Setup(repo => repo.GetAll())
                .Returns(users);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() =>
                _userService.CreateAsync(registerDto));
        }

        [Fact]
        public async Task CreateAsync_WithValidData_CreatesUser()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "new@example.com",
                Password = "password123",
                Role = "User"
            };

            var users = new List<User>().BuildMock();

            _mockUserRepository.Setup(repo => repo.GetAll())
                .Returns(users);
            _mockJwtService.Setup(jwt => jwt.HashPassword("password123"))
                .Returns("hashed_password");
            _mockUserRepository.Setup(repo => repo.InsertAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateAsync(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(registerDto.Email);
            result.Name.Should().Be(registerDto.Name);
            result.PasswordHash.Should().Be("hashed_password");
            _mockUserRepository.Verify(repo => repo.InsertAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeactivateUserAsync_WhenUserExists_DeactivatesUser()
        {
            // Arrange
            var userId = 1;
            var adminId = 2;
            var user = new User { Id = userId, IsActive = true };

            _mockUserRepository.Setup(repo => repo.GetAsync(userId))
                .ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            _mockProductService.Setup(prodService => prodService.HideUserProductsAsync(userId, adminId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.DeactivateUserAsync(userId, adminId);

            // Assert
            result.Should().BeTrue();
            user.IsActive.Should().BeFalse();
            user.DeactivatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            _mockProductService.Verify(prodService => prodService.HideUserProductsAsync(userId, adminId), Times.Once);
        }

        [Fact]
        public async Task ActivateUserAsync_WhenUserExists_ActivatesUser()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, IsActive = false, DeactivatedAt = DateTime.UtcNow };

            _mockUserRepository.Setup(repo => repo.GetAsync(userId))
                .ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            _mockProductService.Setup(prodService => prodService.ShowUserProductsAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.ActivateUserAsync(userId);

            // Assert
            result.Should().BeTrue();
            user.IsActive.Should().BeTrue();
            user.DeactivatedAt.Should().BeNull();
            _mockProductService.Verify(prodService => prodService.ShowUserProductsAsync(userId), Times.Once);
        }
    }
}