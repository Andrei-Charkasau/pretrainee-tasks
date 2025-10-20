using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.Services;

namespace Task_4_1_Library_ControlSystem.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private static List<Book> _books = new List<Book>();
        private static int lastInt = 0; 

        public void Insert(Book book)
        {
            book.Id = lastInt++;
            _books.Add(book);
        }

        public void Update(Book patchBook)
        {
            Book bookToUpdated = _books.FirstOrDefault(book => book.Id == patchBook.Id);
            if (bookToUpdated != null)
            {
                bookToUpdated.Title = patchBook.Title;
                bookToUpdated.PublishedYear = patchBook.PublishedYear;
                bookToUpdated.AuthorId = patchBook.AuthorId;
                bookToUpdated.Id = patchBook.Id;
            }
        }
        public void Delete(int bookId)
        {
            Book bookToDelete = _books.FirstOrDefault(book => book.Id == bookId);
            if (bookToDelete != null)
            {
                _books.Remove(bookToDelete);
            }
        }

        public List<Book> FetchAll()
        {
            return _books;
        }

        public Book Fetch(int bookId)
        {
            return _books.FirstOrDefault(book => book.Id == bookId);
        }
    }
}
