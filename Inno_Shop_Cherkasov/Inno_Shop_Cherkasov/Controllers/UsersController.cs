using Inno_Shop_Cherkasov.Contexts;
using Inno_Shop_Cherkasov.DtoModels;
using Inno_Shop_Cherkasov.Models;
using Inno_Shop_Cherkasov.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inno_Shop_Cherkasov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync(UserDto userDto)
        {
            await _userService.CreateAsync(userDto);
            return Ok("User was registered successfully. [+]");
        }

        [HttpPatch]
        public async Task<ActionResult<string>> UpdateUserAsync(int id, UserDto userDto)
        {
            await _userService.UpdateAsync(id, userDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteUserAsync(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
