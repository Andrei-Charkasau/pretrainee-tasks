using Task_4_1_Library_ControlSystem.Repositories;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;
using System.Runtime.CompilerServices;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        public BookService (IRepository<Book> bookRepository, IRepository<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public void Add(BookDto bookDto)
        {
            try
            {
                if (_authorRepository.Fetch(bookDto.AuthorId) == null)
                {
                    throw new Exception("!!! ERROR: There is no author with such ID. !!!");
                }

                if (string.IsNullOrEmpty(bookDto.Title))
                {
                    throw new Exception("!!! ERROR: Book title must be filled. !!!");
                }

                Book book = new Book();
                book.Title = bookDto.Title;
                book.PublishedYear = bookDto.PublishedYear;
                book.AuthorId = bookDto.AuthorId;

                _bookRepository.Insert(book);
            }
            catch
            {
                throw;
            }
        }

        public void Erase(int bookId)
        {
            var existingBook = _bookRepository.Fetch(bookId);
            if (existingBook == null)
            {
                throw new Exception("!!! ERROR: Book not found. !!!");
            }
            _bookRepository.Delete(bookId);
        }

        public void Modify(int bookIdToUpdate, BookDto bookDto)
        {
            if (string.IsNullOrEmpty(bookDto.Title))
            {
                throw new Exception("!!! ERROR: Book title must be filled. !!!");
            }
            var existingBook = _bookRepository.Fetch(bookIdToUpdate);
            if (existingBook == null)
            {
                throw new Exception("!!! ERROR: Book not found. !!!");
            }

            var patchBook = new Book();
            patchBook.Id = bookIdToUpdate;
            patchBook.Title = bookDto.Title;
            patchBook.PublishedYear = bookDto.PublishedYear;
            patchBook.AuthorId = bookDto.AuthorId;

            _bookRepository.Update(patchBook);
        }

        public List<Book> RetriveAll()
        {
            try
            {
                return _bookRepository.FetchAll();
            }
            catch
            {
                throw;
            }
        }

        public Book Retrive(int bookId)
        {
            try
            {
                return _bookRepository.Fetch(bookId);
            }
            catch
            {
                throw;
            }
        }
    }
}
