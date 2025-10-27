using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Controllers;
using Task_4_1_Library_ControlSystem.Validators;
using Microsoft.EntityFrameworkCore;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class AuthorService : IAuthorService
    {

        private readonly IRepository<Author, int> _authorRepository;

        public AuthorService(IRepository<Author, int> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task CreateAuthorAsync(AuthorDto authorDto)
        {
            authorDto.Name.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Author name must be filled. !!!");

            Author author = new Author();
            author.Name = authorDto.Name;
            author.DateOfBirth = authorDto.DateOfBirth;

            await _authorRepository.InsertAsync(author);
        }

        public async Task DeleteAuthorAsync(int authorId)
        {
            var existingAuthor = _authorRepository.GetAsync(authorId);
            existingAuthor.ThrowExceptionIfNull("!!! ERROR: Author not found. !!!");
            await _authorRepository.DeleteAsync(authorId);
        }

        public async Task UpdateAuthorAsync(int authorId, AuthorDto authorDto)
        {
            authorDto.Name.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Author name must be filled. !!!");
            var existingAuthor = _authorRepository.GetAsync(authorId);
            existingAuthor.ThrowExceptionIfNull("!!! ERROR: Author not found. !!!");

            Author patchAuthor = new Author();
            patchAuthor.Name = authorDto.Name;
            patchAuthor.DateOfBirth = authorDto.DateOfBirth;
            patchAuthor.Id = authorId;

            await _authorRepository.UpdateAsync(patchAuthor);
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _authorRepository.GetAsync(id);
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAll().ToListAsync();
        }

        public async Task<Author> GetAuthorByNameAsync(string authorName)
        {
            return (Author)await _authorRepository.FindAsync(x => x.Name.StartsWith($"{authorName.Trim()}"));
        }

        public async Task<List<AuthorWithBooksCountDto>> GetAllAuthorsWithBooksAmountAsync()
        {
            var authors = _authorRepository.GetAll().Select(x => new AuthorWithBooksCountDto(
                x.Id,
                x.Name,
                x.Books.Count
            ));
            return await authors.ToListAsync();
        }
    }
}
