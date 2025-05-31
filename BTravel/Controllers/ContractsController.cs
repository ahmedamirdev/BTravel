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

        public ContractsController(BTravelDbContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("Add_Contract")]
        public IActionResult Add()
        {
            return View("~/Views/Dashboard/Contracts/Add.cshtml");
        }

        [HttpPost]
        [AuthorizePerRole("Add_Contract")]
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

        [AuthorizePerRole("Add_Contract")]
        public IActionResult Add2()
        {
            return View("~/Views/Dashboard/Contracts/Add2.cshtml");
        }

        [HttpPost]
        [AuthorizePerRole("Add_Contract")]
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

        [AuthorizePerRole("View_Contract_Dashboard")]
        public IActionResult All(int PageIndex = 0, string Search = "")
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
                PageIndex = PageIndex
            };

            var query = new GetAllContracts(request);
            var response = query.GetAll(Search);

            return View("~/Views/Dashboard/Contracts/All.cshtml", response);
        }

        [HttpPost]
        [AuthorizePerRole("UploadFile_Dashboard")]
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
                return RedirectToAction("Details", "Contracts", new { Id = ContractId });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Details", "Contracts", new { Id = ContractId });
            }
        }

        [HttpPost]
        [AuthorizePerRole("Delete_Contract")]
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

        [AuthorizePerRole("View_Contract_Dashboard")]
        public IActionResult Details(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractById(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Dashboard/Contracts/Details.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
        }

        [HttpGet]
        [AuthorizePerRole("Edit_Contract_Dashboard")]
        public IActionResult Edit(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForEdit(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Dashboard/Contracts/Edit.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Contracts"); // redirect to list page after success
            }
        }

        [HttpPost]
        [AuthorizePerRole("Edit_Contract_Dashboard")]
        public IActionResult Edit(ContractEditDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new EditContract(request);
            var response = query.Edit(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Details", "Contracts", new { Id = model.ContractId });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Dashboard/Contracts/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [AuthorizePerRole("DeleteFile_Dashboard")]
        public IActionResult DeleteFile(int FileId)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new DeleteFile(request);
            var response = query.Delete(FileId);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Details", "Contracts", new { Id = response.Data });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Details", "Contracts", new { Id = response.Data });
            }
        }
    }
}