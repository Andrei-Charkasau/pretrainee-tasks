using FluentAssertions;
using InnoShop.Core.Models;
using InnoShop.Core.Services.Services;
using InnoShop.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace InnoShop.Tests.UnitTests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IProductService> _mockProductService;
        private readonly AdminController _adminController;

        public AdminControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockProductService = new Mock<IProductService>();
            _adminController = new AdminController(_mockUserService.Object, _mockProductService.Object);

            SetupAdminContext(1);
        }

        private void SetupAdminContext(int adminId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, adminId.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _adminController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [Fact]
        public async Task DeactivateUser_WithValidId_ReturnsOk()
        {
            // Arrange
            var userId = 2;
            var adminId = 1;

            _mockUserService.Setup(service => service.DeactivateUserAsync(userId, adminId))
                .ReturnsAsync(true);

            // Act
            var result = await _adminController.DeactivateUser(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeactivateUser_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var userId = 999;
            var adminId = 1;

            _mockUserService.Setup(service => service.DeactivateUserAsync(userId, adminId))
                .ReturnsAsync(false);

            // Act
            var result = await _adminController.DeactivateUser(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task ActivateUser_WithValidId_ReturnsOk()
        {
            // Arrange
            var userId = 2;

            _mockUserService.Setup(service => service.ActivateUserAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _adminController.ActivateUser(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task HideProduct_WithValidId_ReturnsOk()
        {
            // Arrange
            var productId = 1;
            var adminId = 1;

            _mockProductService.Setup(service => service.HideProductByAdminAsync(productId, adminId))
                .ReturnsAsync(true);

            // Act
            var result = await _adminController.HideProduct(productId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}