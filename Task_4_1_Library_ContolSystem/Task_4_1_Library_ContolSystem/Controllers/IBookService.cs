using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Controllers
{
    public interface IBookService
    {
        public void CreateBook(BookDto bookDto);
        public void DeleteBook(int id);
        public void UpdateBook(int id, BookDto bookDto);
        public Book GetBookById(int id);
        public List<Book> GetAllBooks();
    }
}
