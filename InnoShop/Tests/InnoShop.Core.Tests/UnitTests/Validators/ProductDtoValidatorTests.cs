using FluentValidation.TestHelper;
using InnoShop.Core.Services.DtoModels;
using InnoShop.Core.Services.Validators;

namespace Innoshop.Core.Tests.UnitTests.Validators
{
    public class ProductDtoValidatorTests
    {
        private readonly ProductDtoValidator _validator;

        public ProductDtoValidatorTests()
        {
            _validator = new ProductDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var productDto = new ProductDto { Name = "", Price = 100 };

            // Act
            var result = _validator.TestValidate(productDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Product name is required");
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Negative()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Test Product", Price = -10 };

            // Act
            var result = _validator.TestValidate(productDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                .WithErrorMessage("Price must be greater than 0");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Valid()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Valid Product",
                Price = 100,
                Availability = true
            };

            // Act
            var result = _validator.TestValidate(productDto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}