using Microsoft.EntityFrameworkCore;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
