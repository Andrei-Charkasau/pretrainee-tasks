using Inno_Shop_Cherkasov.Models;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop_Cherkasov.Contexts
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
