using Microsoft.AspNetCore.Mvc;

namespace SIMS_SE06205.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            // load 1 view hien thi
            return View();
        }
        public IActionResult Test(string name, int id)
        {
            // name, id : bien duoc nhan tu url ngoai trinh duyet
            ViewData["id"] = id;
            ViewData["name"] = name;
            return View();
        }
    }
}
