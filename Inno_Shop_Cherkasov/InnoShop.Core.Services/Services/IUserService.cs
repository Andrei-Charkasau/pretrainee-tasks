using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;

namespace InnoShop.Core.Services.Services
{
    public interface IUserService
    {
        public Task<User> CreateAsync(RegisterDto registerDto);
        public Task UpdateAsync(int userId, UserDto userDto);
        public Task DeleteAsync(int userId);
        public Task<User> GetAsync(int userId);
        public Task<List<User>> GetAllAsync();
        public Task<string> AuthenticateAsync(LoginDto loginDto);
        public Task<bool> ConfirmEmailAsync(string dto);
    }
}
