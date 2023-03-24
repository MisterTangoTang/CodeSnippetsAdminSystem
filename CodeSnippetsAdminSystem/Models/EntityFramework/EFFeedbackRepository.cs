using AdministrationSystem.Eamv.Models.Interfaces;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class EFFeedbackRepository : IFeedbackRepository
    {
        private FeedbackDbContext context;

        public EFFeedbackRepository(FeedbackDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Feedback> Collection { get { return context.Feedbacks; } }

        public void Create(Feedback feedback)
        {
            if (feedback != null)
            {
                context.Add(feedback);
                context.SaveChanges();
            }
        }
    }
}
