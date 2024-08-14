using Microsoft.AspNetCore.Mvc;
using SIMS_SE06205.Models;
using System.Diagnostics;

namespace SIMS_SE06205.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionUserId") == null)
            {
                // N?u ch?a ??ng nh?p, chuy?n h??ng v? trang ??ng nh?p
                return RedirectToAction(nameof(Index), "Login");
            }
            //return Ok("ASP.Net MVC - SE06205");
            return View();
        }

        public IActionResult Hello()
        {
            if (HttpContext.Session.GetString("SessionUserId") == null)
            {
                // N?u ch?a ??ng nh?p, chuy?n h??ng v? trang ??ng nh?p
                return RedirectToAction(nameof(Index), "Login");
            }
            return Ok("Hi you");
        }

        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetString("SessionUserId") == null)
            {
                // N?u ch?a ??ng nh?p, chuy?n h??ng v? trang ??ng nh?p
                return RedirectToAction(nameof(Index), "Login");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404)
                {
                    return View("NotFound");
                }
            }
            return View();
        }
    }
}
