using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIMS_SE06205.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIMS_SE06205.Controllers
{
    public class CourseController : Controller
    {
        private readonly string filePathCourse = @"E:\APDP-BTec-main (1)\data-sims\data-courses.json";

       
        [HttpGet]
        public IActionResult Index()
        {
           
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson) ?? new List<CourseViewModel>();

                var courseModel = new CourseModel
                {
                    CourseLists = courses.Select(c => new CourseViewModel
                    {
                        Id = c.Id,
                        NameCourse = c.NameCourse,
                        Description = c.Description
                    }).ToList()
                };

                return View(courseModel);
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần thiết
                TempData["ErrorMessage"] = "Error loading courses. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            
            return View(new CourseViewModel());
        }

        [HttpPost]
        public IActionResult Add(CourseViewModel courseViewModel)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    string dataJson = System.IO.File.ReadAllText(filePathCourse);
                    var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson) ?? new List<CourseViewModel>();

                    int maxId = courses.Any() ? courses.Max(c => int.Parse(c.Id)) + 1 : 1;
                    courseViewModel.Id = maxId.ToString();

                    courses.Add(courseViewModel);

                    System.IO.File.WriteAllText(filePathCourse, JsonConvert.SerializeObject(courses, Formatting.Indented));
                    TempData["saveStatus"] = true;
                }
                catch (Exception ex)
                {
                    // Log lỗi ở đây nếu cần thiết
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson);

                var itemToDelete = courses?.FirstOrDefault(item => item.Id == id.ToString());
                if (itemToDelete != null)
                {
                    courses.Remove(itemToDelete);
                    System.IO.File.WriteAllText(filePathCourse, JsonConvert.SerializeObject(courses, Formatting.Indented));
                    TempData["DeleteStatus"] = true;
                }
                else
                {
                    TempData["DeleteStatus"] = false;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần thiết
                TempData["DeleteStatus"] = false;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson);
                var itemCourse = courses?.FirstOrDefault(item => item.Id == id.ToString());

                if (itemCourse != null)
                {
                    return View(new CourseViewModel
                    {
                        Id = itemCourse.Id,
                        NameCourse = itemCourse.NameCourse,
                        Description = itemCourse.Description
                    });
                }

                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần thiết
                TempData["ErrorMessage"] = "Error loading course. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Update(CourseViewModel courseModel)
        {
           
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors
                return View(courseModel);
            }

            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathCourse);
                var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson);
                var itemCourse = courses?.FirstOrDefault(item => item.Id == courseModel.Id.ToString());

                if (itemCourse != null)
                {
                    itemCourse.NameCourse = courseModel.NameCourse;
                    itemCourse.Description = courseModel.Description;

                    System.IO.File.WriteAllText(filePathCourse, JsonConvert.SerializeObject(courses, Formatting.Indented));
                    TempData["UpdateStatus"] = true;
                }
                else
                {
                    TempData["UpdateStatus"] = false;
                }
            }
            catch (Exception ex)
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
            string dataJson = System.IO.File.ReadAllText(filePathCourse);
            CourseModel courseModel = new CourseModel();
            courseModel.CourseLists = new List<CourseViewModel>();

     var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson);
     var dataCourse = courses
         .Where(c => c.NameCourse.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                  || c.Description.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                  || c.Id.ToString().Contains(searchTerm))  // Tìm kiếm theo ID
         .ToList();

     if (dataCourse.Count == 0)
     {
         TempData["ErrorMessage"] = "No courses found matching your search criteria.";
                return RedirectToAction(nameof(Index), new { searchTerm = searchTerm });
            }

     foreach (var item in dataCourse)
     {
         courseModel.CourseLists.Add(new CourseViewModel
         {
             Id = item.Id,
             NameCourse = item.NameCourse,
             Description = item.Description
         });
     }
            ViewBag.SearchTerm = searchTerm;
            ViewBag.HasSearched = true;
            return View("Index", courseModel);
 }
    }
}
