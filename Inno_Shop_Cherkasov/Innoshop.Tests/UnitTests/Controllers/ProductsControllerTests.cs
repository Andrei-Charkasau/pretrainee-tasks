using FluentAssertions;
using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;
using InnoShop.Core.Services.Services;
using InnoShop.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace InnoShop.Tests.UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _productsController;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _productsController = new ProductsController(_mockProductService.Object);

            SetupUserContext(1, "User");
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

            _productsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsOkWithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };

            _mockProductService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productsController.GetAllProductsAsync();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetProductAsync_WithValidId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Price = 100 };

            _mockProductService.Setup(s => s.GetAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productsController.GetProductAsync(productId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductAsync_WithInvalidId_ReturnsOkWithNull()
        {
            // Arrange
            var productId = 999;

            _mockProductService.Setup(s => s.GetAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productsController.GetProductAsync(productId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeNull();
        }

        [Fact]
        public async Task CreateProductAsync_WithValidData_ReturnsOk()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "New Product",
                Description = "Product Description",
                Price = 100,
                Availability = true
            };

            _mockProductService.Setup(s => s.CreateAsync(productDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productsController.CreateProductAsync(productDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be("Product was created successfully. [+]");
            _mockProductService.Verify(s => s.CreateAsync(productDto), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "",
                Price = 100,
                Availability = true
            };

            _mockProductService.Setup(s => s.CreateAsync(productDto))
                .ThrowsAsync(new ArgumentException("Product name is required"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productsController.CreateProductAsync(productDto));
        }

        [Fact]
        public async Task UpdateProductAsync_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto
            {
                Name = "Updated Product",
                Price = 150,
                Availability = true
            };

            _mockProductService.Setup(s => s.UpdateAsync(productId, productDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productsController.UpdateProductAsync(productId, productDto);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
            _mockProductService.Verify(s => s.UpdateAsync(productId, productDto), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_WithUnauthorizedAccess_ThrowsException()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto
            {
                Name = "Updated Product",
                Price = 150,
                Availability = true
            };

            _mockProductService.Setup(s => s.UpdateAsync(productId, productDto))
                .ThrowsAsync(new UnauthorizedAccessException("You can only edit your own products"));

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _productsController.UpdateProductAsync(productId, productDto));
        }

        [Fact]
        public async Task DeleteProductAsync_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;

            _mockProductService.Setup(s => s.DeleteAsync(productId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productsController.DeleteProductAsync(productId);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
            _mockProductService.Verify(s => s.DeleteAsync(productId), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_WithUnauthorizedAccess_ThrowsException()
        {
            // Arrange
            var productId = 1;

            _mockProductService.Setup(s => s.DeleteAsync(productId))
                .ThrowsAsync(new UnauthorizedAccessException("You can only delete your own products"));

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _productsController.DeleteProductAsync(productId));
        }

        [Fact]
        public async Task SearchProducts_WithFilters_ReturnsFilteredProducts()
        {
            // Arrange
            var filter = new ProductFilterDto
            {
                SearchTerm = "phone",
                MinPrice = 100,
                MaxPrice = 1000
            };

            var filteredProducts = new List<Product>
            {
                new Product { Id = 1, Name = "iPhone", Price = 500 },
                new Product { Id = 2, Name = "Android Phone", Price = 300 }
            };

            _mockProductService.Setup(s => s.SearchProductsAsync(filter))
                .ReturnsAsync(filteredProducts);

            // Act
            var result = await _productsController.SearchProducts(filter);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(filteredProducts);
        }

        [Fact]
        public async Task SearchProducts_WithNoFilters_ReturnsAllProducts()
        {
            // Arrange
            var filter = new ProductFilterDto();

            var allProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100 },
                new Product { Id = 2, Name = "Product 2", Price = 200 }
            };

            _mockProductService.Setup(s => s.SearchProductsAsync(filter))
                .ReturnsAsync(allProducts);

            // Act
            var result = await _productsController.SearchProducts(filter);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(allProducts);
        }

        [Fact]
        public async Task CreateProductAsync_WhenUserIsAdmin_ReturnsOk()
        {
            // Arrange
            SetupUserContext(1, "Admin");

            var productDto = new ProductDto
            {
                Name = "Admin Product",
                Price = 100,
                Availability = true
            };

            _mockProductService.Setup(s => s.CreateAsync(productDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _productsController.CreateProductAsync(productDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}