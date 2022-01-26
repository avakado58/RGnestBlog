using Microsoft.AspNetCore.Mvc;

namespace RGnestBlog.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //ViewData["Message"] = "Катя мявка";
            ViewBag.Head = "Привет мир!";
            return View();
        }
        public IActionResult Boll()
        {
            return View();
        }
    }
}
