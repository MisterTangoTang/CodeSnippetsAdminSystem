using System.ComponentModel.DataAnnotations;

namespace AdministrationSystem.Eamv.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "Indtast et brugernavn")]
        [MaxLength(40)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Indtast et kodeord")]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Indtast en brugerrolle")]
        public string? UserRole { get; set; }
    }
}
