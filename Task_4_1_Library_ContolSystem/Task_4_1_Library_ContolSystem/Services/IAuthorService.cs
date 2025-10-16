using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Services
{
    public interface IAuthorService
    {
        public void Add(AuthorDto authorDto);
        public void Erase(int id);
        public void Modify(int id, AuthorDto authorDto);
        public Author Retrive(int id);
        public List<Author> RetriveAll();
    }
}
