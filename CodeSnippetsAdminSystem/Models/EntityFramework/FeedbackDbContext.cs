using Microsoft.EntityFrameworkCore;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class FeedbackDbContext : DbContext
    {
        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options) : base(options) { }

        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
