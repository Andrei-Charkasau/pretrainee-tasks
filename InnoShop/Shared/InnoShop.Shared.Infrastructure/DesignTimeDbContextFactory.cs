using InnoShop.Shared.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InnoDbContext>
{
    public InnoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InnoDbContext>();

        optionsBuilder.UseNpgsql("Host=postgres;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword");

        return new InnoDbContext(optionsBuilder.Options);
    }
}