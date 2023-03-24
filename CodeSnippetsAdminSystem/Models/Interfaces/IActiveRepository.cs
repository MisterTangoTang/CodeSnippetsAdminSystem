namespace AdministrationSystem.Eamv.Models.Interfaces
{
    public interface IActiveRepository
    {
        public IQueryable<Activity> Collection { get; }
        public void Create(Activity t);
        public void Delete(int id);
        public Activity GetByID(int id);
        public void Update(Activity t);
        public void ChangeActivityStatus(int id, bool status);
    }
}
