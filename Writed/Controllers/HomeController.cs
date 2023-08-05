using Microsoft.AspNetCore.Mvc;

namespace Writed.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
