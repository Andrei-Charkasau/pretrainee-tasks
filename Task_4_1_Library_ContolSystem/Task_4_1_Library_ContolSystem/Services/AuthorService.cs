using Task_4_1_Library_ControlSystem.Repositories;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class AuthorService : IAuthorService
    {

        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public void Add(AuthorDto authorDto)
        {
            try
            {
                if (authorDto.Name == "")
                {
                    throw new Exception("!!! ERROR: Author name must be filled. !!!");
                }

                Author author = new Author();
                author.Name = authorDto.Name;
                author.DateOfBirth = authorDto.DateOfBirth;

                _authorRepository.Insert(author);
            }
            catch
            {
                throw;
            }
        }

        public void Erase(int authorId)
        {
            var existingAuthor = _authorRepository.Fetch(authorId);
            if (existingAuthor == null)
            {
                throw new Exception("!!! ERROR: Author not found. !!!");
            }
            _authorRepository.Delete(authorId);
        }

        public void Modify(int authorId, AuthorDto authorDto)
        {
            if (string.IsNullOrEmpty(authorDto.Name))
            {
                throw new Exception("!!! ERROR: Author name must be filled. !!!");
            }
            var existingAuthor = _authorRepository.Fetch(authorId);
            if (existingAuthor == null)
            {
                throw new Exception("!!! ERROR: Author not found. !!!");
            }

            Author patchAuthor = new Author();
            patchAuthor.Name = authorDto.Name;
            patchAuthor.DateOfBirth = authorDto.DateOfBirth;
            patchAuthor.Id = authorId;

            _authorRepository.Update(patchAuthor);
        }

        public Author Retrive(int id)
        {
            try
            {
                return _authorRepository.Fetch(id);
            }
            catch
            {
                throw;
            }
        }

        public List<Author> RetriveAll()
        {
            try
            {
                return _authorRepository.FetchAll();
            }
            catch
            {
                throw;
            }
        }
    }
}
