using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SIMS_SE06205.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ClassController : Controller
{
    private string filePathClass = @"E:\APDP-BTec-main (1)\data-sims\class.json";
    private string filePathCourse = @"E:\APDP-BTec-main (1)\data-sims\data-courses.json";
    private string filePathTeacher = @"E:\APDP-BTec-main (1)\data-sims\teacher.json"; // Assuming there's a file for teachers

    // Method to get the list of majors from courses
    private List<SelectListItem> GetMajorList()
    {
        try
        {
            string dataJson = System.IO.File.ReadAllText(filePathCourse);
            var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(dataJson) ?? new List<CourseViewModel>();

            return courses.Select(c => new SelectListItem
            {
                Value = c.NameCourse,
                Text = c.NameCourse
            }).ToList();
        }
        catch
        {
            return new List<SelectListItem>();
        }
    }


    // Method to get the list of teachers
    private List<SelectListItem> GetTeacherList()
    {
        try
        {
            string dataJson = System.IO.File.ReadAllText(filePathTeacher);
            var teachers = JsonConvert.DeserializeObject<List<TeacherViewModel>>(dataJson) ?? new List<TeacherViewModel>();

            return teachers.Select(t => new SelectListItem
            {
                Value = t.Name,
                Text = t.Name
            }).ToList();
        }
        catch
        {
            return new List<SelectListItem>();
        }
    }

    // Method to get the list of slots
    private List<SelectListItem> GetSlotList()
    {
        var slots = Enumerable.Range(1, 6).Select(i => new SelectListItem
        {
            Value = i.ToString(),
            Text = i.ToString()
        }).ToList();

        return slots;
    }


    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            // Read class data
            string dataJson = System.IO.File.ReadAllText(filePathClass);
            var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson) ?? new List<ClassViewModel>();

            var classModel = new ClassModel
            {
                ClassLists = classes
            };

            return View(classModel);
        }
        catch
        {
            TempData["ErrorMessage"] = "An error occurred while loading the class data.";
            return View(new ClassModel());
        }
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.MajorList = GetMajorList();  // Populate dropdown for Major
        ViewBag.TeacherList = GetTeacherList();  // Populate dropdown for Teacher
        ViewBag.SlotList = GetSlotList();  // Populate dropdown for Slot
        return View(new ClassViewModel());
    }

    [HttpPost]
    public IActionResult Add(ClassViewModel classViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                string dataJson = System.IO.File.ReadAllText(filePathClass);
                var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson) ?? new List<ClassViewModel>();

                int maxId = classes.Any() ? classes.Max(c => c.Id) + 1 : 1;

                classes.Add(new ClassViewModel
                {
                    Id = maxId,
                    NameClass = classViewModel.NameClass,
                    Major = classViewModel.Major,
                    Teacher = classViewModel.Teacher,
                    Slot = classViewModel.Slot
                });

                string dtJson = JsonConvert.SerializeObject(classes, Formatting.Indented);
                System.IO.File.WriteAllText(filePathClass, dtJson);
                TempData["SaveStatus"] = true;
            }
            catch
            {
                TempData["SaveStatus"] = false;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.MajorList = GetMajorList();
        ViewBag.TeacherList = GetTeacherList();
        ViewBag.SlotList = GetSlotList();
        return View(classViewModel);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        try
        {
            string dataJson = System.IO.File.ReadAllText(filePathClass);
            var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson) ?? new List<ClassViewModel>();

            var classToDelete = classes.FirstOrDefault(c => c.Id == id);
            if (classToDelete != null)
            {
                classes.Remove(classToDelete);
                string updatedClassJson = JsonConvert.SerializeObject(classes, Formatting.Indented);
                System.IO.File.WriteAllText(filePathClass, updatedClassJson);
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
            string dataJson = System.IO.File.ReadAllText(filePathClass);
            var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson) ?? new List<ClassViewModel>();
            var itemClass = classes.FirstOrDefault(c => c.Id == id);

            if (itemClass != null)
            {
                ViewBag.MajorList = GetMajorList();
                ViewBag.TeacherList = GetTeacherList();
                ViewBag.SlotList = GetSlotList();

                var classModel = new ClassViewModel
                {
                    Id = itemClass.Id,
                    NameClass = itemClass.NameClass,
                    Major = itemClass.Major,
                    Teacher = itemClass.Teacher,
                    Slot = itemClass.Slot
                };

                return View(classModel);
            }

            TempData["ErrorMessage"] = "Class not found.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "An error occurred while loading the class data.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public IActionResult Update(ClassViewModel classModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MajorList = GetMajorList();
            ViewBag.TeacherList = GetTeacherList();
            ViewBag.SlotList = GetSlotList();
            return View(classModel);
        }

        try
        {
            string dataJson = System.IO.File.ReadAllText(filePathClass);
            var classes = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson) ?? new List<ClassViewModel>();
            var itemClass = classes.FirstOrDefault(c => c.Id == classModel.Id);

            if (itemClass != null)
            {
                itemClass.NameClass = classModel.NameClass;
                itemClass.Major = classModel.Major;
                itemClass.Teacher = classModel.Teacher;
                itemClass.Slot = classModel.Slot;

                string updateJson = JsonConvert.SerializeObject(classes, Formatting.Indented);
                System.IO.File.WriteAllText(filePathClass, updateJson);
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
        string dataJson = System.IO.File.ReadAllText(filePathClass);
        ClassModel classModel = new ClassModel();
        classModel.ClassLists = new List<ClassViewModel>();

        var classs = JsonConvert.DeserializeObject<List<ClassViewModel>>(dataJson);
        var dataClass = classs
            .Where(c => c.NameClass.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                     || c.Major.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                     || c.Teacher.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                     || c.Slot.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                     || c.Id.ToString().Contains(searchTerm))  // Tìm kiếm theo ID
            .ToList();

        if (dataClass.Count == 0)
        {
            TempData["ErrorMessage"] = "No class found matching your search criteria.";
            return RedirectToAction(nameof(Index));
        }

        foreach (var item in dataClass)
        {
            classModel.ClassLists.Add(new ClassViewModel
            {
                Id = item.Id,
                NameClass = item.NameClass,
                Major = item.Major,
                Teacher = item.Teacher,
                Slot = item.Slot
                
            });
        }

        ViewBag.SearchTerm = searchTerm;
        ViewBag.HasSearched = true;

        return View("Index", classModel);
    }
}
