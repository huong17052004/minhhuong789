using Microsoft.AspNetCore.Mvc;
using SIMS_SE06205.Models;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SIMS_SE06205.Controllers
{
    public class RegisterController : Controller
    {
        private string filePathDataUser = @"E:\APDP-BTec-main (1)\data-sims\data-user.json";

        [HttpGet]
        public IActionResult Index()
        {
            RegisterViewModel vm = new RegisterViewModel();
            ViewBag.RegisterStatus = TempData["registerStatus"];
            return View(vm);
        }



        [HttpPost]
        public IActionResult Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Read the current data from the JSON file
                string dataJson = System.IO.File.ReadAllText(filePathDataUser);
                var users = JsonConvert.DeserializeObject<List<RegisterViewModel>>(dataJson) ?? new List<RegisterViewModel>();

                // Check if the username already exists
                if (users.Exists(u => u.UserName == model.UserName))
                {
                    TempData["registerStatus"] = "Username already exists!";
                    return View(model);
                }

                // Add the new user to the list
                model.Id = (users.Count + 1).ToString(); // Generate a new ID
                users.Add(model);

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(users, Formatting.Indented);
                System.IO.File.WriteAllText(filePathDataUser, updatedJson);

                TempData["registerStatus"] = "Registration successful! Please log in.!";
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            return View(model);
        }
    }
}