using FluentAssertions;
using FluentValidation.TestHelper;
using InnoShop.Core.DtoModels;
using InnoShop.Core.Validators;
using Xunit;

namespace InnoShop.Tests.UnitTests.Validators
{
    public class LoginDtoValidatorTests
    {
        private readonly LoginDtoValidator _validator;

        public LoginDtoValidatorTests()
        {
            _validator = new LoginDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "", Password = "password123" };

            // Act & Assert
            _validator.TestValidate(loginDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "invalid-email", Password = "password123" };

            // Act & Assert
            _validator.TestValidate(loginDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "" };

            // Act & Assert
            _validator.TestValidate(loginDto)
                .ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password is required");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "123" };

            // Act & Assert
            _validator.TestValidate(loginDto)
                .ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must be at least 6 characters");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Valid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password123" };

            // Act & Assert
            var result = _validator.TestValidate(loginDto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}