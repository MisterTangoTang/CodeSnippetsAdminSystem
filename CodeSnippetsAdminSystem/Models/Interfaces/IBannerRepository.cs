namespace AdministrationSystem.Eamv.Models.Interfaces
{
    public interface IBannerRepository
    {
        public IQueryable<Banner> Collection { get; }
        public void Create(Banner banner);
        public void Delete(int id);
        public Banner GetByID(int id);
        public void ChangeIsActive(int id);
    }
}
