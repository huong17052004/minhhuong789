using Microsoft.AspNetCore.Mvc;
using SIMS_SE06205.Models;
using System;

namespace SIMS_SE06205.Controllers
{
    public class LoginController : Controller
    {
        private string filePathDataUser = @"E:\APDP-BTec-main (1)\data-sims\data-user.json";

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
                        //HttpContext.Session.SetString("SessionRole", user.Role);
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

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Handle other errors
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}"); // Handle 404 errors
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
