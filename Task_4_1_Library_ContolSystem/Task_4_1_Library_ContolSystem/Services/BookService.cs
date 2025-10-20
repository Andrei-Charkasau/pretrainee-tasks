using Task_4_1_Library_ControlSystem.Controllers;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IGuard _guard;

        public BookService (IRepository<Book> bookRepository, IRepository<Author> authorRepository, IGuard guard)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _guard = guard;
        }

        public void CreateBook(BookDto bookDto)
        {
            _guard.AgainstNullOrEmpty(bookDto.Title, "!!! ERROR: Book title must be filled. !!!");
            _guard.AgainstNull(_authorRepository.Get(bookDto.AuthorId), "!!! ERROR: There is no author with such ID. !!!");

            Book book = new Book();
            book.Title = bookDto.Title;
            book.PublishedYear = bookDto.PublishedYear;
            book.AuthorId = bookDto.AuthorId;

            _bookRepository.Insert(book);
        }

        public void DeleteBook(int bookId)
        {
            var existingBook = _bookRepository.Get(bookId);
            _guard.AgainstNull(existingBook, "!!! ERROR: Book not found. !!!");
            _bookRepository.Delete(bookId);
        }

        public void UpdateBook(int bookIdToUpdate, BookDto bookDto)
        {
            _guard.AgainstNullOrEmpty(bookDto.Title, "!!! ERROR: Book title must be filled. !!!");
            var existingBook = _bookRepository.Get(bookIdToUpdate);
            _guard.AgainstNull(existingBook, "!!! ERROR: Book not found. !!!");

            var patchBook = new Book();
            patchBook.Id = bookIdToUpdate;
            patchBook.Title = bookDto.Title;
            patchBook.PublishedYear = bookDto.PublishedYear;
            patchBook.AuthorId = bookDto.AuthorId;

            _bookRepository.Update(patchBook);
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public Book GetBookById(int bookId)
        {
            return _bookRepository.Get(bookId);
        }
    }
}
