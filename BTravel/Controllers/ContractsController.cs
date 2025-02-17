using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
{
    public class ContractsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Dashboard/Contracts/Index.cshtml");
        }
    }
}
