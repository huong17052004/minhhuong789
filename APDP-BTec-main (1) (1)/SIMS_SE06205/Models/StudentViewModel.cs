using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class StudentModel
    {
        public List<StudentViewModel> StudentLists { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public class StudentViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? NameStudent { get; set; }

        [Required(ErrorMessage = "Major is required")]
        public string? Major { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Adress { get; set; }

        [Required(ErrorMessage = "Class is required")]
        public string? Class { get; set; } // Thêm trường Class
    }
}
