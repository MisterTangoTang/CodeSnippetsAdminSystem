namespace AdministrationSystem.Eamv.Models.Interfaces
{
    public interface IFeedbackRepository
    {
        public IQueryable<Feedback> Collection { get; }

        public void Create(Feedback feedback);
    }
}
