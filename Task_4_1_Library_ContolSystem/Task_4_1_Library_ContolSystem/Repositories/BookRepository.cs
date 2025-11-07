using Task_4_1_Library_ControlSystem.Contexts;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public void Insert(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Update(Book patchBook)
        {
            var bookToUpdate = _context.Books.FirstOrDefault(book => book.Id == patchBook.Id);

            if (bookToUpdate != null)
            {
                bookToUpdate.Title = patchBook.Title;
                bookToUpdate.PublishedYear = patchBook.PublishedYear;
                bookToUpdate.AuthorId = patchBook.AuthorId;

                _context.SaveChanges();
            }
        }
        public void Delete(int bookId)
        {
            var bookToDelete = _context.Books.FirstOrDefault(book => book.Id == bookId);

            if (bookToDelete != null)
            {
                _context.Books.Remove(bookToDelete);
                _context.SaveChanges();
            }
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public Book Get(int bookId)
        {
            return _context.Books.FirstOrDefault(book => book.Id == bookId);
        }

        //------------------------------------------Services / Reps continue!
        /*
            public IEnumerable<Book> GetBookByAuthorId(int authorId)
            {
                return _context.Books.Where(book => book.AuthorId == authorId);
            }

            public IEnumerable<Book> GetBooksByYearOfCreation(int publishedYear)
            {
                return _context.Books.Where(book => book.PublishedYear > publishedYear);
            }
        */

        public IQueryable<Book> GetAllNew()
        {
            return _context.Books.AsQueryable();
        }
    }
}
