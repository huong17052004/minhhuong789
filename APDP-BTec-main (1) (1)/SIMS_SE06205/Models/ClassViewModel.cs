using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class ClassModel
    {
        public List<ClassViewModel> ClassLists { get; set; }
    }
    public class ClassViewModel
    {
        [Key]
        public int Id { get; set; } // Represents "id" as an integer

        [Required(ErrorMessage = "Name can not be empty")]
        public string NameClass { get; set; } // Represents "name" as a string

        [Required(ErrorMessage = "Major can not be empty")]
        public string Major { get; set; } // Represents "major" as a string

        [Required(ErrorMessage = "Teacher can not be empty")]
        public string Teacher { get; set; } // Represents "teacher" as a string

        [Required(ErrorMessage = "Slot can not be empty")]
        public string Slot { get; set; } // Represents "slot" as a string
    }
}
