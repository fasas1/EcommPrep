using Ecomm_demo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecomm_demo.Data
{
    public class ApplicationDbContext :DbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Category> Categories { get; set; } = null!;
            public DbSet<Product> Products { get; set; } = null!;
            public DbSet<Customer> Customers { get; set; } = null!;
            public DbSet<Order> Orders { get; set; } = null!;
            public DbSet<OrderItem> OrderItems { get; set; } = null!;
        }
}
