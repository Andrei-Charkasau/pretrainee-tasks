using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private static List<Author> _authors = new List<Author>();
        private static int lastInt;
        public void Insert(Author author)
        {
            author.Id = lastInt++;
            _authors.Add(author);
        }

        public void Update(Author patchAuthor)
        {
            Author authorToUpdate= _authors.FirstOrDefault(author =>  author.Id == patchAuthor.Id); //<--- 6_6
            if (authorToUpdate != null)
            {
                authorToUpdate.DateOfBirth = patchAuthor.DateOfBirth;
                authorToUpdate.Name = patchAuthor.Name;
                authorToUpdate.Id = patchAuthor.Id;
            }
        }
        public void Delete(int authorId)
        {
            Author authorToDelete = _authors.FirstOrDefault(author => author.Id == authorId);
            if (authorToDelete != null)
            {
                _authors.Remove(authorToDelete);
            }
        }

        public List<Author> FetchAll()
        {
            return _authors;
        }

        public Author Fetch(int authorId)
        {
            return _authors.FirstOrDefault(author => author.Id == authorId);
        }
    }
}
