using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
{
    public class ContractsController : Controller
    {
        public IActionResult Add()
        {
            return View("~/Views/Dashboard/Contracts/Add.cshtml");
        }
    }
}
