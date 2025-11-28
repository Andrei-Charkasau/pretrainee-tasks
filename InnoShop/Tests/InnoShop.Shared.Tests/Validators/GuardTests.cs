using InnoShop.Shared.Infrastructure.Exceptions;
using InnoShop.Shared.Infrastructure.Validators;
using Xunit;

namespace InnoShop.Shared.Tests.Validators
{
    public class GuardTests
    {

        [Fact]
        public void ThrowExceptionIfNull_WithNonNullObject_ReturnsSameObject()
        {
            // Arrange
            var testObject = new object();

            // Act
            var result = testObject.ThrowExceptionIfNull("Should not throw");

            // Assert
            Assert.Same(testObject, result);
        }

        [Fact]
        public void ThrowExceptionIfNull_WithNullObject_ThrowsArgumentNullExceptionWithCorrectParamName()
        {
            // Arrange
            string nullString = null;
            var errorMessage = "String is null";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                nullString.ThrowExceptionIfNull(errorMessage));

            // ArgumentNullException обычно содержит имя параметра в ParamName
            Assert.NotNull(exception.ParamName);
        }

        [Theory]
        [InlineData("valid string")]
        [InlineData(" a ")]
        [InlineData("123")]
        public void ThrowExceptionIfNullOrWhiteSpace_WithValidString_ReturnsSameString(string validString)
        {
            // Act
            var result = validString.ThrowExceptionIfNullOrWhiteSpace("Should not throw");

            // Assert
            Assert.Equal(validString, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\n")]
        public void ThrowExceptionIfNullOrWhiteSpace_WithInvalidString_ThrowsArgumentException(string invalidString)
        {
            // Arrange
            var errorMessage = "String is invalid";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                invalidString.ThrowExceptionIfNullOrWhiteSpace(errorMessage));

            Assert.Equal(errorMessage, exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100.50)]
        [InlineData(0.01)]
        public void ThrowExceptionIfNegative_WithNonNegativeNumber_ReturnsSameNumber(decimal number)
        {
            // Act
            var result = number.ThrowExceptionIfNegative("Should not throw");

            // Assert
            Assert.Equal(number, result);
        }

        [Fact]
        public void ThrowBusinessExceptionIf_WithFalseCondition_DoesNotThrow()
        {
            // Arrange
            var condition = false;

            // Act & Assert (no exception)
            Guard.ThrowBusinessExceptionIf(condition, "Should not throw");
        }

        [Fact]
        public void ThrowBusinessExceptionIf_WithTrueCondition_ThrowsBusinessException()
        {
            // Arrange
            var condition = true;
            var errorMessage = "Business rule violation";

            // Act & Assert
            var exception = Assert.Throws<BusinessException>(() =>
                Guard.ThrowBusinessExceptionIf(condition, errorMessage));

            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public void ThrowNotFoundExceptionIfNull_WithNonNullEntity_ReturnsSameEntity()
        {
            // Arrange
            var entity = new object();

            // Act
            var result = entity.ThrowNotFoundExceptionIfNull("TestEntity", 123);

            // Assert
            Assert.Same(entity, result);
        }

        [Fact]
        public void ThrowNotFoundExceptionIfNull_WithNullEntityAndKey_ThrowsNotFoundExceptionWithKey()
        {
            // Arrange
            object nullEntity = null;
            var entityName = "Product";
            var key = 456;

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() =>
                nullEntity.ThrowNotFoundExceptionIfNull(entityName, key));

            var expectedMessage = $"Entity 'Product' with key '456' was not found.";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ThrowNotFoundExceptionIfNull_WithNullEntityWithoutKey_ThrowsNotFoundExceptionWithoutKey()
        {
            // Arrange
            object nullEntity = null;
            var entityName = "User";

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() =>
                nullEntity.ThrowNotFoundExceptionIfNull(entityName));

            var expectedMessage = "User was not found.";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ThrowNotFoundExceptionIfNull_WithNullEntityAndNullKey_ThrowsNotFoundExceptionWithoutKey()
        {
            // Arrange
            object nullEntity = null;
            var entityName = "Order";
            object nullKey = null;

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() =>
                nullEntity.ThrowNotFoundExceptionIfNull(entityName, nullKey));

            var expectedMessage = "Order was not found.";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ThrowNotFoundExceptionIfNull_WithNullEntityAndStringKey_ThrowsNotFoundExceptionWithKey()
        {
            // Arrange
            object nullEntity = null;
            var entityName = "Category";
            var key = "electronics";

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() =>
                nullEntity.ThrowNotFoundExceptionIfNull(entityName, key));

            var expectedMessage = $"Entity 'Category' with key 'electronics' was not found.";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void RealWorldScenario_ProductValidation_WorksCorrectly()
        {
            // Arrange
            var product = new { Name = "Test Product", Price = 25.99m };

            // Act & Assert (no exceptions)
            var validatedName = product.Name.ThrowExceptionIfNullOrWhiteSpace("Product name is required");
            var validatedPrice = product.Price.ThrowExceptionIfNegative("Price cannot be negative");

            Assert.Equal("Test Product", validatedName);
            Assert.Equal(25.99m, validatedPrice);
        }
    }
}