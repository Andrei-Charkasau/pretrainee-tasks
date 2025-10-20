using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Controllers
{
    public interface IAuthorService
    {
        public void CreateAuthor(AuthorDto authorDto);
        public void DeleteAuthor(int id);
        public void UpdateAuthor(int id, AuthorDto authorDto);
        public Author GetAuthorById(int id);
        public List<Author> GetAllAuthors();
    }
}
