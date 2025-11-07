using Inno_Shop_Cherkasov.DtoModels;
using Inno_Shop_Cherkasov.Models;

namespace Inno_Shop_Cherkasov.Services
{
    public interface IUserService
    {
        public Task CreateAsync(UserDto userDto);
        public Task UpdateAsync(int userId, UserDto userDto);
        public Task DeleteAsync(int userId);
        public Task<User> GetAsync(int userId);
        public Task<List<User>> GetAllAsync();
    }
}
