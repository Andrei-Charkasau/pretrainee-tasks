using Inno_Shop_Cherkasov.Models;
using Microsoft.EntityFrameworkCore;

namespace Inno_Shop_Cherkasov.Contexts
{
    public class ShopContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }
    }
}
