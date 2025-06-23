using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.CommonUser.Queries;
using BTravel.BL.Services.Contracts.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Security.Auth;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly BTravelDbContext _context;

        public UsersController(BTravelDbContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("Add_CommonUser")]
        public IActionResult Add()
        {
            return View("~/Views/Dashboard/Users/Add.cshtml");
        }

        [HttpPost]
        [AuthorizePerRole("Add_CommonUser")]
        public IActionResult Add(CommonUserAddDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new AddCommonUser(request);
            var response = query.Add(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("All", "Users"); // redirect to list page after success
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Dashboard/Users/Add.cshtml", model);
            }
        }

        [AuthorizePerRole("View_CommonUser")]
        public IActionResult All(int PageIndex = 0)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
                PageIndex = PageIndex
            };

            var query = new GetAllCommonUsers(request);
            var response = query.GetAll(string.Empty);

            return View("~/Views/Dashboard/Users/All.cshtml", response);
        }

        [HttpGet]
        [AuthorizePerRole("View_CommonUser")]
        public IActionResult Search(string text)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetAllCommonUsers(request);
            var response = query.GetAll(text);

            return View("~/Views/Dashboard/Users/All.cshtml", response);
        }

        [HttpPost]
        [AuthorizePerRole("Delete_CommonUser")]
        public IActionResult Delete(int CommonUserId)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new DeleteCommonUser(request);
            var response = query.Delete(CommonUserId);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("All", "Users"); // redirect to list page after success
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Users"); // redirect to list page after success
            }
        }

        [AuthorizePerRole("View_CommonUser")]
        public IActionResult Details(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetCommonUserById(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Dashboard/Users/Details.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Users"); // redirect to list page after success
            }
        }

        [HttpGet]
        [AuthorizePerRole("Edit_CommonUser")]
        public IActionResult Edit(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetCommonUserByIdForEdit(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Dashboard/Users/Edit.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("All", "Users"); // redirect to list page after success
            }
        }

        [HttpPost]
        [AuthorizePerRole("Edit_CommonUser")]
        public IActionResult Edit(CommonUserAddDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new EditCommonUser(request);
            var response = query.Edit(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Details", "Users", new { Id = model.CommonUserId });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Dashboard/Users/Edit.cshtml", model);
            }
        }

        //[AuthorizePerRole("Change_Password")]
        public IActionResult ChangePassword()
        {
            return View("~/Views/Dashboard/Users/ChangePassword.cshtml");
        }
    }
}