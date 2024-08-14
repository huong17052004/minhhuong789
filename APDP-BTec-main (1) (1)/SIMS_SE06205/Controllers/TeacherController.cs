using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_SE06205.Models;

namespace SIMS_SE06205.Controllers
{
    public class TeacherController : Controller
    {
        private string filePathTeacher = @"E:\APDP-BTec-main (1)\data-sims\teacher.json";
        private string filePathCourse = @"E:\APDP-BTec-main (1)\data-sims\data-courses.json ";

        // Method to get the list of courses as SelectListItem
        private List<SelectListItem> GetCourseList()
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson) ?? new List<CourseViewModel>();

                return courses.Select(c => new SelectListItem
                {
                    Value = c.NameCourse,  // Value to be stored
                    Text = c.NameCourse    // Text to display in dropdown
                }).ToList();
            }
            catch (Exception)
            {
                // Log error if needed
                return new List<SelectListItem>();
            }
        }

       

        [HttpGet]
        public IActionResult Index()
        {
            
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson) ?? new List<TeacherViewModel>();

                string courseJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(courseJson) ?? new List<CourseViewModel>();

                foreach (var teacher in teachers)
                {
                    if (courses.All(c => c.NameCourse != teacher.Major))
                    {
                        teacher.Major = "Unknown"; // Or null if preferred
                    }
                }

                var updatedTeacherJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                System.IO.File.WriteAllText(filePathTeacher, updatedTeacherJson);

                var teacherModel = new TeacherModel
                {
                    TeacherLists = teachers
                };

                return View(teacherModel);
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while loading the teacher data.";
                return View(new TeacherModel { TeacherLists = new List<TeacherViewModel>() });
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            
            ViewBag.CourseList = GetCourseList();  // Pass the course list to ViewBag
            return View(new TeacherViewModel());
        }

        [HttpPost]
        public IActionResult Add(TeacherViewModel teacherViewModel)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                    var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson) ?? new List<TeacherViewModel>();

                    int maxId = teachers.Any() ? int.Parse(teachers.Max(s => s.Id)) + 1 : 1;
                    string idIncrement = maxId.ToString();

                    teachers.Add(new TeacherViewModel
                    {
                        Id = idIncrement,
                        Name = teacherViewModel.Name,
                        Birthday = teacherViewModel.Birthday,
                        Major = teacherViewModel.Major,
                        Gender = teacherViewModel.Gender,
                        Address = teacherViewModel.Address
                    });

                    string dtJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathTeacher, dtJson);
                    TempData["saveStatus"] = true;
                }
                catch
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CourseList = GetCourseList();  // Pass the course list back if there's an error
            return View(teacherViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
           
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson) ?? new List<TeacherViewModel>();

                string courseJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(courseJson) ?? new List<CourseViewModel>();

                var teacherToDelete = teachers.FirstOrDefault(t => t.Id == id.ToString());
                if (teacherToDelete != null)
                {
                    teachers.Remove(teacherToDelete);

                    string updatedTeacherJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathTeacher, updatedTeacherJson);

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
                string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson) ?? new List<TeacherViewModel>();
                var itemTeacher = teachers.FirstOrDefault(item => item.Id == id.ToString());

                if (itemTeacher != null)
                {
                    ViewBag.CourseList = GetCourseList();  // Pass the course list to ViewBag

                    var teacherModel = new TeacherViewModel
                    {
                        Id = itemTeacher.Id,
                        Name = itemTeacher.Name,
                        Birthday = itemTeacher.Birthday,
                        Major = itemTeacher.Major,
                        Gender = itemTeacher.Gender,
                        Address = itemTeacher.Address
                    };

                    return View(teacherModel);
                }

                TempData["ErrorMessage"] = "Teacher not found.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while loading the teacher data.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Update(TeacherViewModel teacherModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseList = GetCourseList();  // Pass the course list again if validation fails
                return View(teacherModel);
            }

            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathTeacher);
                var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson) ?? new List<TeacherViewModel>();
                var itemTeacher = teachers.FirstOrDefault(item => item.Id == teacherModel.Id.ToString());

                if (itemTeacher != null)
                {
                    itemTeacher.Name = teacherModel.Name;
                    itemTeacher.Birthday = teacherModel.Birthday;
                    itemTeacher.Major = teacherModel.Major;
                    itemTeacher.Gender = teacherModel.Gender;
                    itemTeacher.Address = teacherModel.Address;

                    string updateJson = JsonConvert.SerializeObject(teachers, Formatting.Indented);
                    System.IO.File.WriteAllText(filePathTeacher, updateJson);
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
            string dataJson = System.IO.File.ReadAllText(filePathTeacher);
            TeacherModel stuModel = new TeacherModel();
            stuModel.TeacherLists = new List<TeacherViewModel>();

            var teacher = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson);
            var dataTeacher = teacher
                .Where(c => c.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Major.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Birthday.ToString().Contains(searchTerm)
                         || c.Gender.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Address.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         || c.Id.ToString().Contains(searchTerm))  // Tìm kiếm theo ID
                .ToList();

            if (dataTeacher.Count == 0)
            {
                TempData["ErrorMessage"] = "No teacher found matching your search criteria.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var item in dataTeacher)
            {
                stuModel.TeacherLists.Add(new TeacherViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Birthday = item.Birthday,
                    Major = item.Major,
                    Gender = item.Gender,
                    Address = item.Address
                });
            }
            ViewBag.SearchTerm = searchTerm;
            ViewBag.HasSearched = true;
            return View("Index", stuModel);
        }
    }
}
