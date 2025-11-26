using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InnoShop.Core.Services.Services;
using System.Security.Claims;

namespace InnoShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public AdminController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        private int GetCurrentAdminId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); //Логируем Айди Админа, который де/активировал юзера| прятал/показывал продукт (чтобы видеть кто и когда)

        [HttpPost("users/{userId}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var adminId = GetCurrentAdminId();
            var success = await _userService.DeactivateUserAsync(userId, adminId);

            if (!success) return BadRequest("User not found");

            return Ok("User deactivated and products hidden");
        }

        [HttpPost("users/{userId}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var success = await _userService.ActivateUserAsync(userId);

            if (!success) return BadRequest("User not found");

            return Ok("User activated and products restored");
        }

        [HttpPost("products/{productId}/hide")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HideProduct(int productId)
        {
            var adminId = GetCurrentAdminId();
            var success = await _productService.HideProductByAdminAsync(productId, adminId);

            if (!success) return BadRequest("Product not found");

            return Ok("Product hidden");
        }

        [HttpPost("products/{productId}/show")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShowProduct(int productId)
        {
            var success = await _productService.ShowProductByAdminAsync(productId);

            if (!success) return BadRequest("Product not found");

            return Ok("Product visible");
        }
    }
}
