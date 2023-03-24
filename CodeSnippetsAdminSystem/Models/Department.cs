using System.ComponentModel.DataAnnotations;

namespace AdministrationSystem.Eamv.Models
{
    public class Department
    { 
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Afdelingsnavn mangler")]
        [MaxLength(40)]
        public string DepartmentName { get; set; }
    }
}
