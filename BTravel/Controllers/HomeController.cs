using System.Diagnostics;
using BTravel.DAL.Entities;

using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
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
            List<Contract> data = new List<Contract>();

            data.Add(new Contract
            {
                ContractId = 1,
                HotelName = "shiraton",

            });
            data.Add(new Contract
            {
                ContractId = 2,
                HotelName = "sofitel",

            });
            data.Add(new Contract
            {
                ContractId = 3,
                HotelName = "helton",

            });


            return View(data);
        }
        public IActionResult Test()
        {
            return View();
        }



        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
