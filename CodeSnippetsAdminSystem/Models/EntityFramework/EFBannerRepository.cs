using AdministrationSystem.Eamv.Models.Interfaces;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class EFBannerRepository : IBannerRepository
    {
        private MainDbContext context;
        public EFBannerRepository(MainDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Banner> Collection { get { return context.Banners; } }

        public void ChangeIsActive(int id)
        {
            Banner banner = GetByID(id);

            if (banner != null)
            {
                if (banner.IsActive == false)
                {
                    banner.IsActive = true;
                    context.SaveChanges();
                }
                else if (banner.IsActive == true)
                {
                    banner.IsActive = false;
                    context.SaveChanges();
                }
            }
        }

        public void Create(Banner banner)
        {
            context.Add(banner);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Banner banner = GetByID(id);

            Console.WriteLine(id);

            if (banner != null)
            {
                context.Remove(banner);
                context.SaveChanges();
            }
        }

        public Banner GetByID(int id)
        {
            foreach (Banner a in context.Banners)
            {
                if (id == a.BannerID)
                {
                    return a;
                }
            }
            return null;
        }
    }
}
