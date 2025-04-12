using BTravel.BL.Services.CommonUser.Queries;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Security.Auth;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using Microsoft.AspNetCore.Mvc;

namespace BTravel.Controllers
{
    public class UsersController : Controller
    {
        private readonly BTravelDbContext _context;

        public UsersController(BTravelDbContext dbcontext)
        {
            _context = dbcontext;
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

            var query = new GetAllCommonUsers(request);
            var response = query.GetAll(new PublicRequest());

            return View("~/Views/Dashboard/Users/All.cshtml", response);
        }
    }
}
