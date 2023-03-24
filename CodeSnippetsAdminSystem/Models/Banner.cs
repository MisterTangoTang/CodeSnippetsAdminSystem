using System.ComponentModel.DataAnnotations;

namespace AdministrationSystem.Eamv.Models
{
    public class Banner
    {
        public int BannerID { get; set; }
        [Required]
        public string BannerName { get; set; }
        [Required]
        public string BannerDescription { get; set; }
        [Required]
        public Department Department { get; set; }
        public bool IsActive { get; set; }
    }
}
