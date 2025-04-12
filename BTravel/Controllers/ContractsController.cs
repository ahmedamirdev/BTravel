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
using BTravel.DAL.Entities;
using BTravel.BL.Services.Contracts.Commands;
using BTravel.BL.Services.Contracts.Queries;
using System.Threading.Tasks;

namespace BTravel.Controllers
{
    [Authorize]
    public class ContractsController : Controller
    {
        private readonly BTravelDbContext _context;

        public ContractsController(BTravelDbContext dbcontext)
        {
            _context = dbcontext;
        }

        public IActionResult Add()
        {
            return View("~/Views/Dashboard/Contracts/Add.cshtml");
        }

        [HttpPost]
        public IActionResult Add(ContractAddDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new AddContract(request);
            var response = query.Add(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Dashboard/Contracts/Add.cshtml", model);
            }
        }

        public IActionResult Add2()
        {
            return View("~/Views/Dashboard/Contracts/Add2.cshtml");
        }

        [HttpPost]
        public IActionResult Add2(ContractAddDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new AddContract(request);
            var response = query.Add(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Dashboard/Contracts/Add2.cshtml", model);
            }
        }

        public IActionResult All(int PageIndex = 0)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
                PageIndex = PageIndex
            };

            var query = new GetAllContracts(request);
            var response = query.GetAll(new PublicRequest());

            return View("~/Views/Dashboard/Contracts/All.cshtml", response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(int ContractId, IFormFile xfile)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new AddContractFile(request);
            var response = await query.AddFile(ContractId, xfile);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
        }

        [HttpPost]
        public IActionResult Delete(int ContractId)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new DeleteContract(request);
            var response = query.Delete(ContractId);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
        }
    }
}