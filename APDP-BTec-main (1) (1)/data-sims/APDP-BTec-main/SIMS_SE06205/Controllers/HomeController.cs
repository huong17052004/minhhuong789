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
            //return Ok("ASP.Net MVC - SE06205");
            return View();
        }

        public IActionResult Hello()
        {
            return Ok("Hi you");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
