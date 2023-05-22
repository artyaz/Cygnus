using Microsoft.EntityFrameworkCore;
namespace Cygnus.Data;

public class AppDbContext : DbContext
{
    
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Add DbSet properties for your entities here
}