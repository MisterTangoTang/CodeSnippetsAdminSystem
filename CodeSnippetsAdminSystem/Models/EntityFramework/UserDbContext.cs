using Microsoft.EntityFrameworkCore;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
