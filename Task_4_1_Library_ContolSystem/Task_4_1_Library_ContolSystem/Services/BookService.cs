using Microsoft.EntityFrameworkCore;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.Repositories;
using Task_4_1_Library_ControlSystem.Validators;

namespace Task_4_1_Library_ControlSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book, int> _bookRepository;
        private readonly IRepository<Author, int> _authorRepository;

        public BookService (IRepository<Book, int> bookRepository, IRepository<Author, int> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task CreateBookAsync(BookDto bookDto)
        {
            bookDto.Title.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Book title must be filled. !!!");
            var author = await _authorRepository.GetAsync(bookDto.AuthorId);
            author.ThrowExceptionIfNull("!!! ERROR: There is no author with such ID. !!!");

            Book book = new Book()
            {
                Title = bookDto.Title.Trim(),
                PublishedYear = bookDto.PublishedYear,
                AuthorId = bookDto.AuthorId
            };

            await _bookRepository.InsertAsync(book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var existingBook = await _bookRepository.GetAsync(bookId);
            existingBook.ThrowExceptionIfNull("!!! ERROR: Book not found. !!!");
            await _bookRepository.DeleteAsync(bookId);
        }

        public async Task UpdateBookAsync(int bookIdToUpdate, BookDto bookDto)
        {
            bookDto.Title.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Book title must be filled. !!!");
            var existingBook = await _bookRepository.GetAsync(bookIdToUpdate);
            existingBook.ThrowExceptionIfNull("!!! ERROR: Book not found. !!!");

            var patchBook = new Book()
            {
               Id = bookIdToUpdate,
                Title = bookDto.Title,
                PublishedYear = bookDto.PublishedYear,
                AuthorId = bookDto.AuthorId
            };

            await _bookRepository.UpdateAsync(patchBook);
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAll().ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _bookRepository.GetAsync(bookId);
        }

        public async Task<List<Book>> GetBooksByAuthorIdAsync(int authorId)
        {
            var existingAuthor = await _authorRepository.GetAsync(authorId);
            existingAuthor.ThrowExceptionIfNull("!!! ERROR: Author not found. !!!");
            var collection = await _bookRepository.FindAsync(x => x.AuthorId == authorId);
            return await collection.ToListAsync();
        }

        public async Task<List<Book>> GetBooksFromPublishYearToNowAsync(int publishingYear)
        {
            Guard.ThrowExceptionIfNegativeYear(publishingYear, "!!! ERROR: Publishing Year must be positive. !!!");
            var collection = await _bookRepository.FindAsync(x => x.PublishedYear > publishingYear);
            return await collection.ToListAsync();
        }

        public async Task<List<Book>> GetBooksByTitleAsync(string bookTitle)
        {
            bookTitle.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Book title must be filled. !!!");
            var collection = await _bookRepository.FindAsync(x => x.Title.StartsWith($"{bookTitle.Trim()}"));
            return await collection.ToListAsync();
        }
    }
}
