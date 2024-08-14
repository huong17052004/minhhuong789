using System.ComponentModel.DataAnnotations;

namespace SIMS_SE06205.Models
{
    public class CourseModel
    {
        public List<CourseViewModel> CourseLists { get; set; }
    }
    public class CourseViewModel
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Course's Name can be not empty")]
        public string NameCourse { get; set; }

        public string? Description { get; set; }

        //public CourseViewModel(string id, string name, string des)
        //{
        //    Id = id;
        //    NameCourse = name;
        //    Description = des;
        //}
    }
}
