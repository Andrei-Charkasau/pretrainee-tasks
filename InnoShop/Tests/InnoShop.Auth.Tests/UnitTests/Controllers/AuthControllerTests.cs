using FluentAssertions;
using InnoShop.Auth.Services.DtoModels;
using InnoShop.Auth.Services.Services;
using InnoShop.Shared.Domain.Models;
using InnoShop.Shared.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InnoShop.Auth.Tests.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockJwtService = new Mock<IJwtService>();
            _mockEmailService = new Mock<IEmailService>();

            _authController = new AuthController(
                _mockUserService.Object,
                _mockJwtService.Object,
                _mockEmailService.Object);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123",
                Role = "User"
            };

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                EmailConfirmationToken = "token123"
            };

            _mockUserService.Setup(service => service.CreateAsync(registerDto))
                .ReturnsAsync(user);
            _mockEmailService.Setup(service => service.SendConfirmationEmailAsync(user.Email, user.EmailConfirmationToken))
                .ReturnsAsync(true);

            // Act
            var result = await _authController.Register(registerDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "existing@example.com",
                Password = "password123",
                Role = "User"
            };

            _mockUserService.Setup(service => service.CreateAsync(registerDto))
                .ThrowsAsync(new BusinessException("User with this email already exists"));

            // Act
            var result = async () => await _authController.Register(registerDto);

            // Assert
            await Assert.ThrowsAsync<BusinessException>(result);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedToken = "Bearer jwt_token";

            _mockUserService.Setup(s => s.AuthenticateAsync(loginDto))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _authController.Login(loginDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(new { Token = expectedToken });
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "wrong_password"
            };

            _mockUserService.Setup(s => s.AuthenticateAsync(loginDto))
                .ReturnsAsync((string)null);

            // Act
            var result = await _authController.Login(loginDto);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult.Value.Should().Be("Invalid email or password");
        }

        [Fact]
        public async Task ConfirmEmail_WithValidToken_ReturnsOk()
        {
            // Arrange
            var confirmDto = new ConfirmEmailDto { Token = "valid_token" };

            _mockUserService.Setup(s => s.ConfirmEmailAsync("valid_token"))
                .ReturnsAsync(true);

            // Act
            var result = await _authController.ConfirmEmail(confirmDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ConfirmEmail_WithInvalidToken_ReturnsBadRequest()
        {
            // Arrange
            var confirmDto = new ConfirmEmailDto { Token = "invalid_token" };

            _mockUserService.Setup(s => s.ConfirmEmailAsync("invalid_token"))
                .ReturnsAsync(false);

            // Act
            var result = await _authController.ConfirmEmail(confirmDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task ForgotPassword_WithExistingEmail_ReturnsOk()
        {
            // Arrange
            var forgotDto = new ForgotPasswordDto { Email = "test@example.com" };

            _mockUserService.Setup(s => s.GeneratePasswordResetTokenAsync("test@example.com"))
                .ReturnsAsync("reset_token");
            _mockEmailService.Setup(s => s.SendPasswordResetEmailAsync("test@example.com", "reset_token"))
                .ReturnsAsync(true);

            // Act
            var result = await _authController.ForgotPassword(forgotDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ForgotPassword_WithNonExistingEmail_ReturnsOk() // Security - don't reveal if email exists
        {
            // Arrange
            var forgotDto = new ForgotPasswordDto { Email = "nonexisting@example.com" };

            _mockUserService.Setup(s => s.GeneratePasswordResetTokenAsync("nonexisting@example.com"))
                .ReturnsAsync((string)null);

            // Act
            var result = await _authController.ForgotPassword(forgotDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Still returns OK for security
        }
    }
}