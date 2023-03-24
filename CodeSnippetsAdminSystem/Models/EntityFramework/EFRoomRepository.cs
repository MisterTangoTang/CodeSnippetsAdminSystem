using AdministrationSystem.Eamv.Models.Interfaces;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class EFRoomRepository : IRepositoryCrud<Room>
    {
        private MainDbContext context;

        public EFRoomRepository(MainDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Room> Collection { get { return context.Rooms; } }

        public void Create(Room room)
        {
            context.Add(room);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Room room = GetByID(id);

            if (room != null)
            {
                context.Remove(room);
                context.SaveChanges();
            }
        }

        public Room GetByID(int id)
        {
            foreach (Room room in context.Rooms)
            {
                if (id == room.RoomID)
                {
                    return room;
                }
            }
            return null;
        }

        public void Update(Room room)
        {
            context.Update(room);
            context.SaveChanges();
        }
    }
}
