using Microsoft.EntityFrameworkCore;
using Task_4_1_Library_ControlSystem.Models;

namespace Task_4_1_Library_ControlSystem.Contexts
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }
    }
}
