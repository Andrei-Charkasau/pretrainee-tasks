using InnoShop.Core.DtoModels;
using InnoShop.Core.Services.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _userService.AuthenticateAsync(loginDto);

        if (token == null)
            return Unauthorized("Invalid email or password");

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var userDto = new UserDto
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Role = registerDto.Role
            };

            await _userService.CreateAsync(userDto);
            return Ok("User registered successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}