using Task_4_1_Library_ControlSystem.Controllers;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.Validators;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        Guard guard = new Guard();
        public BookService (IRepository<Book> bookRepository, IRepository<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public void CreateBook(BookDto bookDto)
        {
            guard.AgainstNull(_authorRepository.Fetch(bookDto.AuthorId), "!!! ERROR: There is no author with such ID. !!!");
            guard.AgainstNullOrEmpty(bookDto.Title, "!!! ERROR: Book title must be filled. !!!");

                Book book = new Book();
                book.Title = bookDto.Title;
                book.PublishedYear = bookDto.PublishedYear;
                book.AuthorId = bookDto.AuthorId;

                _bookRepository.Insert(book);
        }

        public void DeleteBook(int bookId)
        {
            var existingBook = _bookRepository.Fetch(bookId);
            guard.AgainstNull(existingBook, "!!! ERROR: Book not found. !!!");
            _bookRepository.Delete(bookId);
        }

        public void UpdateBook(int bookIdToUpdate, BookDto bookDto)
        {
            guard.AgainstNullOrEmpty(bookDto.Title, "!!! ERROR: Book title must be filled. !!!");
            var existingBook = _bookRepository.Fetch(bookIdToUpdate);
            guard.AgainstNull(existingBook, "!!! ERROR: Book not found. !!!");

            var patchBook = new Book();
            patchBook.Id = bookIdToUpdate;
            patchBook.Title = bookDto.Title;
            patchBook.PublishedYear = bookDto.PublishedYear;
            patchBook.AuthorId = bookDto.AuthorId;

            _bookRepository.Update(patchBook);
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.FetchAll();
        }

        public Book GetBookById(int bookId)
        {
            return _bookRepository.Fetch(bookId);
        }
    }
}
