using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Controllers
{
    public interface IBookService
    {
        public Task CreateBookAsync(BookDto bookDto);
        public Task DeleteBookAsync(int id);
        public Task UpdateBookAsync(int id, BookDto bookDto);
        public Task<Book> GetBookByIdAsync(int id);
        public Task<List<Book>> GetBooksByAuthorIdAsync(int id);
        public Task<List<Book>> GetBooksFromPublishYearToNowAsync(int publishingYear);
        public Task<List<Book>> GetAllBooksAsync();
    }
}
