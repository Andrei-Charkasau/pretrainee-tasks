using FluentAssertions;
using FluentValidation.TestHelper;
using InnoShop.Core.DtoModels;
using InnoShop.Core.Validators;
using Xunit;

namespace InnoShop.Tests.UnitTests.Validators
{
    public class UserDtoValidatorTests
    {
        private readonly UserDtoValidator _validator;

        public UserDtoValidatorTests()
        {
            _validator = new UserDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "",
                Email = "test@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Too_Short()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "A",
                Email = "test@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name must be between 2 and 50 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Too_Long()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = new string('A', 51),
                Email = "test@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name must be between 2 and 50 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email is required");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "invalid-email",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Too_Long()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = new string('a', 95) + "@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email cannot exceed 100 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Role_Is_Empty()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Role = ""
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Role)
                .WithErrorMessage("Role is required");
        }

        [Fact]
        public void Should_Have_Error_When_Role_Is_Invalid()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Role = "Moderator"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Role)
                .WithErrorMessage("Role must be either 'User' or 'Admin'");
        }

        [Theory]
        [InlineData("User")]
        [InlineData("Admin")]
        public void Should_Accept_Valid_Roles(string role)
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Role = role
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldNotHaveValidationErrorFor(x => x.Role);
        }

        [Theory]
        [InlineData("John Doe")]
        [InlineData("Anna-Maria")]
        [InlineData("Jean Paul")]
        [InlineData("X Æ A-12")]
        public void Should_Accept_Valid_Names(string name)
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = name,
                Email = "test@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.co.uk")]
        [InlineData("user+tag@example.org")]
        [InlineData("user_name@domain.com")]
        public void Should_Accept_Valid_Emails(string email)
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = email,
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("@domain.com")]
        public void Should_Reject_Invalid_Emails(string email)
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Test User",
                Email = email,
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Valid User",
                Email = "valid@example.com",
                Role = "User"
            };

            // Act & Assert
            var result = _validator.TestValidate(userDto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Minimum_Length()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Al",
                Email = "test@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Name_Is_Maximum_Length()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = new string('A', 50),
                Email = "test@example.com",
                Role = "User"
            };

            // Act & Assert
            _validator.TestValidate(userDto)
                .ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}