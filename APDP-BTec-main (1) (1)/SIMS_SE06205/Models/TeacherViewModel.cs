using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class TeacherModel
    {
        public List<TeacherViewModel> TeacherLists { get; set; }
    }

    public class TeacherViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Teacher's name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Birthday cannot be empty")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Major cannot be empty")]
        public string Major { get; set; }

        [Required(ErrorMessage = "Gender cannot be empty")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }
    }

}
