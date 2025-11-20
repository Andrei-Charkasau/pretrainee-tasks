using InnoShop.Core.DtoModels;
using InnoShop.Core.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthController(IUserService userService, IJwtService jwtService, IEmailService emailService)
    {
        _userService = userService;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _userService.AuthenticateAsync(loginDto);

        if (token == null)
        {
            return Unauthorized("Invalid email or password");
        }

        return Ok(new { Token = token });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await _userService.CreateAsync(registerDto);
        await _emailService.SendConfirmationEmailAsync(user.Email, user.EmailConfirmationToken);
        return Ok("Registration successful! Please check your email address to confirm your account.");
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
    {
        var success = await _userService.ConfirmEmailAsync(dto.Token);

        if (!success)
        {
            return BadRequest("Invalid or expired token!");
        }
        else
        {
            return Ok("Your email has been confirmed! You can now log in to your account.");
        }    
    }
}