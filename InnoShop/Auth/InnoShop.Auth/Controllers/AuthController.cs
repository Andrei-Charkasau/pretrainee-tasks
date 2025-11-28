using InnoShop.Auth.Services.DtoModels;
using InnoShop.Auth.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace InnoShop.Auth.Controllers
{
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

            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            if (user?.IsActive == false)
            {
                return Unauthorized("Account deactivated");
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
                return BadRequest("ERROR: Invalid or expired token. !!!");
            }
            else
            {
                return Ok("Your email has been confirmed! You can now log in to your account.");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var resetToken = await _userService.GeneratePasswordResetTokenAsync(dto.Email);

            if (resetToken == null)
            {
                return Ok(new { message = "Instructions has been sent to E-Mail." });
            }

            var emailSent = await _emailService.SendPasswordResetEmailAsync(dto.Email, resetToken); // Отправляем email

            if (!emailSent)
            {
                return StatusCode(500, new { error = "ERROR: Failed to send email. !!!" });
            }

            return Ok(new { message = "Password Recovery instructions has been sent to E-Mail." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _userService.ResetPasswordAsync(dto.Token, dto.NewPassword);

            if (!success)
            {
                return BadRequest(new { error = "ERROR: Invalid or expired token. !!!" });
            }

            return Ok(new { message = "Password changed succesfully." });
        }
    }
}