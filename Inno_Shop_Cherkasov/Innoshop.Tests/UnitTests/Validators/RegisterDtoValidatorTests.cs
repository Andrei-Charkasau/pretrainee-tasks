using FluentAssertions;
using FluentValidation.TestHelper;
using InnoShop.Core.DtoModels;
using InnoShop.Core.Validators;
using Xunit;

namespace InnoShop.Tests.UnitTests.Validators
{
    public class RegisterDtoValidatorTests
    {
        private readonly RegisterDtoValidator _validator;

        public RegisterDtoValidatorTests()
        {
            _validator = new RegisterDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "",
                Email = "test@example.com",
                Password = "password123",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(registerDto)
                .ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "invalid-email",
                Password = "password123",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(registerDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "",
                Password = "password123",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(registerDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "123",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(registerDto)
                .ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must be at least 6 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Role_Is_Invalid()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123",
                Role = "Moderator"
            };

            // Act & Assert
            _validator.TestValidate(registerDto)
                .ShouldHaveValidationErrorFor(x => x.Role)
                .WithErrorMessage("Role must be either 'User' or 'Admin'");
        }

        [Theory]
        [InlineData("User")]
        [InlineData("Admin")]
        public void Should_Accept_Valid_Roles(string role)
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123",
                Role = role
            };

            // Act & Assert
            _validator.TestValidate(registerDto)
                .ShouldNotHaveValidationErrorFor(x => x.Role);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Valid()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123",
                Role = "User"
            };

            // Act & Assert
            var result = _validator.TestValidate(registerDto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}