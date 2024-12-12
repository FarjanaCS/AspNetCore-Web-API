using Microsoft.AspNetCore.Mvc;

namespace EmQL_1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
