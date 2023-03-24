using System.ComponentModel.DataAnnotations;

namespace AdministrationSystem.Eamv.Models
{
    public class Activity
    {
        public int Activityid { get; set; }
        [Required(ErrorMessage = "Titel mangler")]
        public string? Title { get; set; }
        public string? ByWhom { get; set; }
        [Required(ErrorMessage = "Dato(er) mangler")]
        public DateTime date { get; set; }
        [Required(ErrorMessage = "Sluttid mangler")]
        public string? EndTime { get; set; }
        [Required(ErrorMessage = "Starttid mangler")]
        public string? StartTime { get; set; }
        [Required(ErrorMessage = "afdeling mangler")]
        public Department? Department { get; set; }
        [Required(ErrorMessage = "Lokale(r) mangler")]
        public List<ActivityRoom> Rooms { get; set; }
        public bool IsCancelled { get; set; } = false;

        public static Activity Copy(Activity activity)
        {
            Activity a = new Activity();
            a.Title = activity.Title;
            a.ByWhom = activity.ByWhom;
            a.date = activity.date;
            a.EndTime = activity.EndTime;
            a.StartTime = activity.StartTime;
            a.Department = activity.Department;
            a.Rooms = ActivityRoom.ActivityRoomCopy(activity.Rooms);
            a.IsCancelled = activity.IsCancelled;
            return a;
        }
    }
}

