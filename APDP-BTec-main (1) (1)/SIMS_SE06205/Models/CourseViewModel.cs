using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class CourseModel
    {
        public List<CourseViewModel> CourseLists { get; set; }
        public string SearchTerm { get; internal set; }
    }
    public class CourseViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "NameCourse is required.")]
        public string NameCourse { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        //public CourseViewModel(string id, string name, string des)
        //{
        //    Id = id;
        //    NameCourse = name;
        //    Description = des;
        //}
    }
}
