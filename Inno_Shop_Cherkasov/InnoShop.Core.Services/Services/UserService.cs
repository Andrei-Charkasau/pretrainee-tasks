using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Repositories;
using InnoShop.Core.Services.Validators;
using Microsoft.EntityFrameworkCore;


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

        public async Task CreateAsync(UserDto userDto)
        {
            userDto.Name.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's NAME must be filled. !!!");
            userDto.Email.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's E-MAIL must be filled. !!!");
            userDto.Role.ThrowExceptionIfNullOrWhiteSpace("ERROR: User's E-MAIL must be filled. !!!");

            User user = new User()
            {
                Name = userDto.Name.Trim(),
                Email = userDto.Email.Trim(),
                Role = userDto.Role.Trim()
            };
            await _userRepository.InsertAsync(user);
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

            if (user == null)
                return null;

            return _jwtService.GenerateToken(user);
        }
    }
}
