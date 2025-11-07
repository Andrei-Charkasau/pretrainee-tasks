using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Services
{
    public interface IAuthorService
    {
        public Task CreateAuthorAsync(AuthorDto authorDto);
        public Task DeleteAuthorAsync(int id);
        public Task UpdateAuthorAsync(int id, AuthorDto authorDto);
        public Task<Author> GetAuthorByIdAsync(int id);
        public Task<List<AuthorWithBooksCountDto>> GetAllAuthorsWithBooksAmountAsync();
        public Task<Author> GetAuthorByNameAsync(string authorName);
        public Task<List<Author>> GetAllAuthorsAsync();
    }
}
