using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;

namespace InnoShop.Core.Services.Services
{
    public interface IUserService
    {
        public Task CreateAsync(UserDto userDto);
        public Task UpdateAsync(int userId, UserDto userDto);
        public Task DeleteAsync(int userId);
        public Task<User> GetAsync(int userId);
        public Task<List<User>> GetAllAsync();
        public Task<string> AuthenticateAsync(LoginDto loginDto);
    }
}
