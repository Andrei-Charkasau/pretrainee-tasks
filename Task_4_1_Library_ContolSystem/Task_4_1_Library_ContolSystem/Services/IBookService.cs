using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Services
{
    public interface IBookService
    {
        public void Add(BookDto bookDto);
        public void Erase(int id);
        public void Modify(int id, BookDto bookDto);
        public Book Retrive(int id);
        public List<Book> RetriveAll();
    }
}
