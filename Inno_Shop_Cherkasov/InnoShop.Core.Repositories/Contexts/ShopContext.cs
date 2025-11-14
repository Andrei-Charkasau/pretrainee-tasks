using InnoShop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.Core.Repositories.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }
    }
}
