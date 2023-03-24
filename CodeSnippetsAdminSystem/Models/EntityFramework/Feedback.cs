using System.ComponentModel.DataAnnotations;

namespace AdministrationSystem.Eamv.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        [Required(ErrorMessage = "Please enter ")]
        public string FeedbackDisc { get; set; }
    }
}
