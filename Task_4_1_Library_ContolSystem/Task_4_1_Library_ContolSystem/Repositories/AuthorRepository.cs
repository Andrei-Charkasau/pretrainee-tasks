using Microsoft.EntityFrameworkCore;
using Task_4_1_Library_ControlSystem.Contexts;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.Services;

namespace Task_4_1_Library_ControlSystem.Repositories
{
    public class AuthorRepository : EntityRepository<Author, int>
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }

        public void Insert(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void Update(Author patchAuthor)
        {
            var authorToUpdate = _context.Authors.FirstOrDefault(author => author.Id == patchAuthor.Id);
            if (authorToUpdate != null)
            {
                authorToUpdate.DateOfBirth = patchAuthor.DateOfBirth;
                authorToUpdate.Name = patchAuthor.Name;
                authorToUpdate.Id = patchAuthor.Id;
                _context.SaveChanges();
            }
        }
        public void Delete(int authorId)
        {
            var authorToDelete = _context.Authors.FirstOrDefault(author => author.Id == authorId);
            if (authorToDelete != null)
            {
                _context.Authors.Remove(authorToDelete);
                _context.SaveChanges();
            }
        }

        public List<Author> GetAll()
        {
            return _context.Authors.Include(a => a.Books).ToList();
        }

        public Author Get(int authorId)
        {
            return _context.Authors.Include(a => a.Books).FirstOrDefault(author => author.Id == authorId);
        }
    }
}
