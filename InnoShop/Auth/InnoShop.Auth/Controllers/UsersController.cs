using InnoShop.Auth.Services.DtoModels;
using InnoShop.Auth.Services.Services;
using InnoShop.Shared.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUserAsync(int userId)
        {
            var user = await _userService.GetAsync(userId);
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> CreateUserAsync(RegisterDto registerDto)
        {
            await _userService.CreateAsync(registerDto);
            return Ok("User was registered successfully. [+]");
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UpdateUserAsync(int id, UserDto userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> DeleteUserAsync(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
