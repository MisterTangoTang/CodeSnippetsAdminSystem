using System.ComponentModel.DataAnnotations;

namespace AdministrationSystem.Eamv.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        [Required(ErrorMessage = "Lokalenavn mangler")]
        [MaxLength(40)]
        public string RoomName { get; set; }
        [Required(ErrorMessage = "Afdelingsnavn mangler")]
        public Department Department { get; set; }
    }

}
