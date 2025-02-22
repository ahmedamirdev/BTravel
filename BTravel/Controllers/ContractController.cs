using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
{
    public class ContractController : Controller
    {
        public IActionResult Add()
        {
            return View("~/Views/Shared/_LayoutDashboard.cshtml");
        }
    }
}
