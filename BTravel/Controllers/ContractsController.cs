using BTravel.BL.Services.Security.Auth;
using BTravel.CommonDefinitions.DTOs.Auth;
using BTravel.CommonDefinitions.Requests;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTravel.DAL;
using BTravel.CommonDefinitions.DTOs.Contract;
using Microsoft.AspNetCore.Authorization;

namespace BTravel.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly BTravelDbContext _dbContext;

        public ContractsController(BTravelDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public IActionResult Add()
        {
            return View("~/Views/Dashboard/Contracts/Add.cshtml");
        }

        public IActionResult Add2()
        {
            return View("~/Views/Dashboard/Contracts/Add2.cshtml");
        }

        [HttpPost]
        public  IActionResult AddContract(ContractAddDTO model)
        {
            //var request = new BaseRequest();
            //request.Context = _dbContext;

            //var query = new Login(request);
            //var response = query.CheckCredentials(model);

            return View();
        }

        
    }
}