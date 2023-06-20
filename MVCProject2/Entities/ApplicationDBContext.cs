using Microsoft.EntityFrameworkCore;
using MVCProject2.Models;

namespace MVCProject2.Entities
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base (options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
