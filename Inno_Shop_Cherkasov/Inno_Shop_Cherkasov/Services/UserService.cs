using Inno_Shop_Cherkasov.DtoModels;
using Inno_Shop_Cherkasov.Models;
using Inno_Shop_Cherkasov.Repositories;
using Inno_Shop_Cherkasov.Validators;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop_Cherkasov.Services
{
    public class UserService : IUserService
    {

        private readonly IRepository<User, int> _userRepository;

        public UserService(IRepository<User,int> repository)
        {
            _userRepository = repository;
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

            User patchUser = new User()
            {
                Id = userId,
                Email = userDto.Email,
                Name = userDto.Name,
                Role = userDto.Role
            };

            await _userRepository.UpdateAsync(patchUser);

        }
    }
}
