using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Controllers;
using Task_4_1_Library_ControlSystem.Validators;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class AuthorService : IAuthorService
    {

        private readonly IRepository<Author> _authorRepository;
        Guard guard = new Guard();

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public void CreateAuthor(AuthorDto authorDto)
        {
            guard.AgainstNullOrEmpty(authorDto.Name, "!!! ERROR: Author name must be filled. !!!");

            Author author = new Author();
            author.Name = authorDto.Name;
            author.DateOfBirth = authorDto.DateOfBirth;

            _authorRepository.Insert(author);
        }

        public void DeleteAuthor(int authorId)
        {
            var existingAuthor = _authorRepository.Fetch(authorId);
            guard.AgainstNull(existingAuthor, "!!! ERROR: Author not found. !!!");
            _authorRepository.Delete(authorId);
        }

        public void UpdateAuthor(int authorId, AuthorDto authorDto)
        {
            guard.AgainstNullOrEmpty(authorDto.Name, "!!! ERROR: Author name must be filled. !!!");
            var existingAuthor = _authorRepository.Fetch(authorId);
            guard.AgainstNull(existingAuthor, "!!! ERROR: Author not found. !!!");

            Author patchAuthor = new Author();
            patchAuthor.Name = authorDto.Name;
            patchAuthor.DateOfBirth = authorDto.DateOfBirth;
            patchAuthor.Id = authorId;

            _authorRepository.Update(patchAuthor);
        }

        public Author GetAuthorById(int id)
        {
            return _authorRepository.Fetch(id);
        }

        public List<Author> GetAllAuthors()
        {
            return _authorRepository.FetchAll();
        }
    }
}
