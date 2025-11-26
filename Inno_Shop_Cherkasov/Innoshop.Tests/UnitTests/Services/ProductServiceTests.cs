using FluentAssertions;
using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Repositories;
using InnoShop.Core.Services.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace InnoShop.Tests.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product, int>> _mockProductRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IRepository<Product, int>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _productService = new ProductService(
                _mockProductRepository.Object,
                _mockHttpContextAccessor.Object);
        }

        private void SetupHttpContext(int userId, string role = "User")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.User).Returns(claimsPrincipal);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
        }

        [Fact]
        public async Task CreateAsync_WithValidData_CreatesProduct()
        {
            // Arrange
            SetupHttpContext(1);
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
                Availability = true
            };

            _mockProductRepository.Setup(repo => repo.InsertAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.CreateAsync(productDto);

            // Assert
            _mockProductRepository.Verify(repo =>
                repo.InsertAsync(It.Is<Product>(product =>
                    product.Name == productDto.Name &&
                    product.Price == productDto.Price &&
                    product.CreatorId == 1)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenUserIsOwner_UpdatesProduct()
        {
            // Arrange
            var productId = 1;
            var userId = 1;
            SetupHttpContext(userId);

            var existingProduct = new Product { Id = productId, CreatorId = userId, Name = "Old Name" };
            var productDto = new ProductDto { Name = "New Name", Description = "", Price = 200, Availability = true };

            _mockProductRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateAsync(productId, productDto);

            // Assert
            existingProduct.Name.Should().Be("New Name");
            existingProduct.Price.Should().Be(200);
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenUserIsNotOwner_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var productId = 1;
            var userId = 2;
            SetupHttpContext(userId);

            var existingProduct = new Product { Id = productId, CreatorId = 1 };
            var productDto = new ProductDto { Name = "New Name", Price = 200, Availability = true };

            _mockProductRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync(existingProduct);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _productService.UpdateAsync(productId, productDto));
        }

        [Fact]
        public async Task UpdateAsync_WhenAdmin_CanUpdateAnyProduct()
        {
            // Arrange
            var productId = 1;
            var adminId = 2;
            SetupHttpContext(adminId, "Admin");

            var existingProduct = new Product { Id = productId, CreatorId = 1 };
            var productDto = new ProductDto { Name = "New Name", Description = "", Price = 200, Availability = true };

            _mockProductRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync(existingProduct);
            _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateAsync(productId, productDto);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenProductIsHidden_ReturnsNull()
        {
            // Arrange
            var productId = 1;
            var hiddenProduct = new Product { Id = productId, IsHidden = true };

            _mockProductRepository.Setup(repo => repo.GetAsync(productId))
                .ReturnsAsync(hiddenProduct);

            // Act
            var result = await _productService.GetAsync(productId);

            // Assert
            result.Should().BeNull();
        }
    }
}