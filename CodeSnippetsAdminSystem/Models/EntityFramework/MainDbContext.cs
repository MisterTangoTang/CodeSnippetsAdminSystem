using Microsoft.EntityFrameworkCore;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Banner> Banners { get; set; }
    }
}
