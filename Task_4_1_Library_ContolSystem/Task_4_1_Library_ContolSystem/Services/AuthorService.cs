using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Controllers;
using Task_4_1_Library_ControlSystem.Validators;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class AuthorService : IAuthorService
    {

        private readonly IRepository<Author> _authorRepository;
        private readonly IGuard _guard;

        public AuthorService(IRepository<Author> authorRepository, IGuard guard)
        {
            _authorRepository = authorRepository;
            _guard = guard;
        }

        public void CreateAuthor(AuthorDto authorDto)
        {
            _guard.AgainstNullOrEmpty(authorDto.Name, "!!! ERROR: Author name must be filled. !!!");

            Author author = new Author();
            author.Name = authorDto.Name;
            author.DateOfBirth = authorDto.DateOfBirth;

            _authorRepository.Insert(author);
        }

        public void DeleteAuthor(int authorId)
        {
            var existingAuthor = _authorRepository.Get(authorId);
            _guard.AgainstNull(existingAuthor, "!!! ERROR: Author not found. !!!");
            _authorRepository.Delete(authorId);
        }

        public void UpdateAuthor(int authorId, AuthorDto authorDto)
        {
            _guard.AgainstNullOrEmpty(authorDto.Name, "!!! ERROR: Author name must be filled. !!!");
            var existingAuthor = _authorRepository.Get(authorId);
            _guard.AgainstNull(existingAuthor, "!!! ERROR: Author not found. !!!");

            Author patchAuthor = new Author();
            patchAuthor.Name = authorDto.Name;
            patchAuthor.DateOfBirth = authorDto.DateOfBirth;
            patchAuthor.Id = authorId;

            _authorRepository.Update(patchAuthor);
        }

        public Author GetAuthorById(int id)
        {
            return _authorRepository.Get(id);
        }

        public List<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll();
        }
    }
}
