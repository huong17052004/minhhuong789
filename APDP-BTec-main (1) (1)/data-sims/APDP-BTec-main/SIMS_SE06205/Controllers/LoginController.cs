using Microsoft.AspNetCore.Mvc;
using SIMS_SE06205.Models;
using System;

namespace SIMS_SE06205.Controllers
{
    public class LoginController : Controller
    {
        private string filePathDataUser = @"C:\CSharp\SIMS\data-sims\\data-user.json";

        [HttpGet]
        public IActionResult Index()
        {
            LoginViewModel vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string username = model.UserName.Trim();
                string password = model.Password.Trim();
                string dataJson = System.IO.File.ReadAllText(filePathDataUser);

                // kiem tra username va password co ton tai trong dataJson hay khong ?
                var people = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LoginViewModel>>(dataJson);
                var user =  (from p in people
                            where p.UserName == username && p.Password == password
                            select p).FirstOrDefault();
                if (user == null)
                {
                    TempData["loginStatus"] = false;
                }
                else
                {
                    TempData["loginStatus"] = true;
                    // luu thong tin nguoi dung vao session
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
                    {
                        HttpContext.Session.SetString("SessionUserId", user.Id);
                        HttpContext.Session.SetString("SessionUsername", user.UserName);
                        HttpContext.Session.SetString("SessionRole", user.Role);
                        HttpContext.Session.SetString("SessionEmail", user.Email);
                    }
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // xoa session da tao ra o login
            // quay ve trang dang nhap
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                // xoa session
                HttpContext.Session.Remove("SessionUserId");
                HttpContext.Session.Remove("SessionUsername");
                HttpContext.Session.Remove("SessionRole");
                HttpContext.Session.Remove("SessionEmail");
            }
            // quay ve trang dang nhap
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
