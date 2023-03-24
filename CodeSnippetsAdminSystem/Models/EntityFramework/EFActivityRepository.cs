using AdministrationSystem.Eamv.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class EFActivityRepository : IActiveRepository
    {
        private MainDbContext context;

        public EFActivityRepository(MainDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Activity> Collection { get { return context.Activities; } }

        public void Create(Activity e)
        {
            context.Add(e);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Activity activity = context.Activities.Include(a => a.Rooms).First(a => a.Activityid == id);

            if (activity != null)
            {
                context.Remove(activity);
                context.RemoveRange(activity.Rooms);
                context.SaveChanges();
            }
        }

        public Activity GetByID(int id)
        {
            foreach (Activity a in context.Activities)
            {
                if (a.Activityid == id)
                    return a;
            }
            return null;
        }

        public void Update(Activity t)
        {
            // First needs to clear all previous rooms in the ActivityRoom db.table for the object
            Activity tempactivity = context.Activities.Include(a => a.Rooms).AsNoTracking().First(a => a.Activityid == t.Activityid);
            if (tempactivity != null)
                context.RemoveRange(tempactivity.Rooms);

            context.Update(t);
            context.SaveChanges();
        }

        public void ChangeActivityStatus(int id, bool status)
        {
            Activity activity = GetByID(id);

            if (activity != null)
            {
                activity.IsCancelled = status;
                context.Update(activity);
                context.SaveChanges();
            }
        }

    }
}

