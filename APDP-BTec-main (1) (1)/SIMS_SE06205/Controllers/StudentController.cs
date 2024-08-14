
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SIMS_SE06205.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIMS_SE06205.Controllers
{
    public class StudentController : Controller
    {
        private string filePathStudent = @"E:\APDP-BTec-main (1)\data-sims\data-student.json";
        private string filePathCourse = @"E:\APDP-BTec-main (1)\data-sims\data-courses.json";
        private string filePathClass = @"E:\APDP-BTec-main (1)\data-sims\class.json";
        private const int PageSize = 5;
        // Phương thức lấy danh sách khóa học và chuyển thành SelectListItem
        private List<SelectListItem> GetCourseList()
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson) ?? new List<CourseViewModel>();

                return courses.Select(c => new SelectListItem
                {
                    Value = c.NameCourse,  // Giá trị được lưu vào Major
                    Text = c.NameCourse    // Giá trị hiển thị trong dropdown
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        // Phương thức lấy danh sách lớp học và chuyển thành SelectListItem
        private List<SelectListItem> GetClassList()
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathClass);
                var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson) ?? new List<ClassViewModel>();

                return classes.Select(c => new SelectListItem
                {
                    Value = c.NameClass,  // Giá trị lưu vào Class
                    Text = c.NameClass    // Giá trị hiển thị trong dropdown
                }).ToList();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        

        [HttpGet]
        public IActionResult Index()
        {
           
            try
            {
                // Đọc dữ liệu sinh viên
                string dataJson = System.IO.File.ReadAllText(filePathStudent);
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson) ?? new List<StudentViewModel>();

                // Đọc dữ liệu khóa học
                string courseJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(courseJson) ?? new List<CourseViewModel>();

                // Cập nhật các sinh viên có Major không còn hợp lệ
                foreach (var student in students)
                {
                    if (courses.All(c => c.NameCourse != student.Major))
                    {
                        student.Major = "Unknown"; // Hoặc null nếu muốn
                    }
                }

                // Ghi lại dữ liệu sinh viên với các thay đổi
                var updatedStudentJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                System.IO.File.WriteAllText(filePathStudent, updatedStudentJson);

                // Chuyển dữ liệu đến View
                var studentModel = new StudentModel
                {
                    StudentLists = students
                };

                return View(studentModel);
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while loading the student data.";
                return View(new StudentModel());
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
           
            ViewBag.CourseList = GetCourseList();
            ViewBag.ClassList = GetClassList();  // Truyền danh sách lớp vào ViewBag
            return View(new StudentViewModel());
        }

        [HttpPost]
        public IActionResult Add(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string dataJson = System.IO.File.ReadAllText(filePathStudent);
                    var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson) ?? new List<StudentViewModel>();

                    int maxId = students.Any() ? int.Parse(students.Max(s => s.Id)) + 1 : 1;
                    string idIncrement = maxId.ToString();

                    students.Add(new StudentViewModel
                    {
                        Id = idIncrement,
                        Code = studentViewModel.Code,
                        NameStudent = studentViewModel.NameStudent,
                        Major = studentViewModel.Major,
                        Gender = studentViewModel.Gender,
                        Adress = studentViewModel.Adress,
                        Class = studentViewModel.Class  // Lưu trường "class"
                    });

                    string dtJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathStudent, dtJson);
                    TempData["saveStatus"] = true;
                }
                catch
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CourseList = GetCourseList();
            ViewBag.ClassList = GetClassList();  // Truyền lại danh sách lớp nếu có lỗi
            return View(studentViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                // Đọc dữ liệu sinh viên
                string dataJson = System.IO.File.ReadAllText(filePathStudent);
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson) ?? new List<StudentViewModel>();

                // Tìm sinh viên để xóa
                var studentToDelete = students.FirstOrDefault(s => s.Id == id.ToString());
                if (studentToDelete != null)
                {
                    // Xóa sinh viên
                    students.Remove(studentToDelete);
                    string updatedStudentJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathStudent, updatedStudentJson);

                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch
            {
                TempData["DeleteStatus"] = false;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathStudent);
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson) ?? new List<StudentViewModel>();
                var itemStudent = students.FirstOrDefault(item => item.Id == id.ToString());

                if (itemStudent != null)
                {
                    ViewBag.CourseList = GetCourseList();
                    ViewBag.ClassList = GetClassList();  // Truyền danh sách lớp vào ViewBag

                    var studentModel = new StudentViewModel
                    {
                        Id = itemStudent.Id,
                        Code = itemStudent.Code,
                        NameStudent = itemStudent.NameStudent,
                        Major = itemStudent.Major,
                        Gender = itemStudent.Gender,
                        Adress = itemStudent.Adress,
                        Class = itemStudent.Class  // Lấy giá trị class từ dữ liệu
                    };

                    return View(studentModel);
                }

                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while loading the student data.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Update(StudentViewModel studentModel)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.CourseList = GetCourseList();
                ViewBag.ClassList = GetClassList();
                return View(studentModel);
            }

            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathStudent);
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson) ?? new List<StudentViewModel>();
                var itemStudent = students.FirstOrDefault(item => item.Id == studentModel.Id.ToString());

                if (itemStudent != null)
                {
                    itemStudent.Code = studentModel.Code;
                    itemStudent.NameStudent = studentModel.NameStudent;
                    itemStudent.Major = studentModel.Major;
                    itemStudent.Gender = studentModel.Gender;
                    itemStudent.Adress = studentModel.Adress;
                    itemStudent.Class = studentModel.Class;

                    string updateJson = JsonConvert.SerializeObject(students, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathStudent, updateJson);
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch
            {
                TempData["UpdateStatus"] = false;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
          
            if (string.IsNullOrEmpty(searchTerm))
            {
                TempData["ErrorMessage"] = "Please enter a search term.";
                return RedirectToAction(nameof(Index));
            }
            searchTerm = searchTerm.Trim();
            string dataJson = System.IO.File.ReadAllText(filePathStudent);
            StudentModel stuModel = new StudentModel();
            stuModel.StudentLists = new List<StudentViewModel>();

            var student = JsonConvert.DeserializeObject<List<StudentViewModel>>(dataJson);
            var dataStudent = student
                .Where(c => c.NameStudent.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Major.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Code.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Gender.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Adress.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Class.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Id.ToString().Contains(searchTerm))  // Tìm kiếm theo ID
                .ToList();

            if (dataStudent.Count == 0)
            {
                TempData["ErrorMessage"] = "No student found matching your search criteria.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var item in dataStudent)
            {
                stuModel.StudentLists.Add(new StudentViewModel
                {
                    Id = item.Id,
                    NameStudent = item.NameStudent,
                    Code = item.Code,
                    Major = item.Major,
                    Gender = item.Gender,
                    Adress = item.Adress,
                    Class = item.Class
                });
            }
            ViewBag.SearchTerm = searchTerm;
            ViewBag.HasSearched = true;
            return View("Index", stuModel);
        }
    }
}
