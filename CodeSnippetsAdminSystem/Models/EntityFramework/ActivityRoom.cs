namespace AdministrationSystem.Eamv.Models
{
    public class ActivityRoom
    {
        public int ActivityRoomID { get; set; }
        public Room room { get; set; }

        public static List<ActivityRoom> ActivityRoomCopy(List<ActivityRoom> rooms)
        {
            List<ActivityRoom> roomsCopy = new List<ActivityRoom>();

            foreach (var r in rooms)
            {
                ActivityRoom room = new ActivityRoom();
                room.room = r.room;
                roomsCopy.Add(room);
            }

            return roomsCopy;
        }
    }

}

