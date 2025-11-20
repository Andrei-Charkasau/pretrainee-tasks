using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Repositories;
using InnoShop.Core.Services.Validators;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


namespace InnoShop.Core.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IRepository<User, int> _userRepository;

        public UserService(IRepository<User,int> repository, IJwtService jwtService)
        {
            _userRepository = repository;
            _jwtService = jwtService;
        }

        public async Task<User> CreateAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("ERROR: User with this E-MAIL ALREADY EXISTS. !!!");
            }
            registerDto.Name.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's NAME must be filled. !!!");
            registerDto.Email.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's E-MAIL must be filled. !!!");
            registerDto.Role.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's E-MAIL must be filled. !!!");

            var confirmationToken = GenerateConfirmationToken();

            User user = new User()
            {
                Name = registerDto.Name.Trim(),
                Email = registerDto.Email.Trim(),
                Role = registerDto.Role.Trim(),
                PasswordHash = _jwtService.HashPassword(registerDto.Password.Trim()),
                IsEmailConfirmed = false,
                EmailConfirmationToken = confirmationToken,
                EmailConfirmationTokenExpires = DateTime.UtcNow.AddDays(1) //
            };
            await _userRepository.InsertAsync(user);
            return user;
        }

        public async Task DeleteAsync(int userId)
        {
            var existingUser = await _userRepository.GetAsync(userId);
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<User> GetAsync(int userId)
        {
            return await _userRepository.GetAsync(userId);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAll().ToListAsync();
        }

        public async Task UpdateAsync(int userId, UserDto userDto)
        {
            userDto.Name.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's NAME must be filled. !!!");
            userDto.Email.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's E-MAIL must be filled. !!!");
            userDto.Role.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's E-MAIL must be filled. !!!");

            var existingUser = await _userRepository.GetAsync(userId);
            existingUser.ThrowExceptionIfNull("ERROR: User not found. !!!");

            existingUser.Id = userId;
            existingUser.Email = userDto.Email;
            existingUser.Name = userDto.Name;
            existingUser.Role = userDto.Role;

            await _userRepository.UpdateAsync(existingUser);

        }
        public async Task<string> AuthenticateAsync(LoginDto loginDto)
        {
            var users = await _userRepository.GetAll().ToListAsync();
            var user = users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null || !user.IsEmailConfirmed || !_jwtService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            return "Bearer " + _jwtService.GenerateToken(user);
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.EmailConfirmationToken == token && 
                                                                               x.EmailConfirmationTokenExpires > DateTime.UtcNow);

            if (user == null) return false;

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpires = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        private string GenerateConfirmationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
