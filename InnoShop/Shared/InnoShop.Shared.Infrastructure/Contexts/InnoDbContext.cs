using InnoShop.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.Shared.Infrastructure.Contexts
{
    public class InnoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public InnoDbContext(DbContextOptions<InnoDbContext> options) : base(options)
        {

        }
    }
}
