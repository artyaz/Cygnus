using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cygnus.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartProduct> CartProducts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Add DbSet properties for your entities here
}