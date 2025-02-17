using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
