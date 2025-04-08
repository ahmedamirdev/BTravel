using BTravel.BL.Services.Security.Auth;
using System.Security.Claims;
using BTravel.CommonDefinitions.DTOs.Auth;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTravel.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly BTravelDbContext _context;

        public DashboardController(BTravelDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var request = new BaseRequest();
            request.Context = _context;

            var query = new Login(request);
            var response = query.CheckCredentials(model);

            if (response.Success)
            {
                var claims = new[]
                {
                    new Claim("UserID", response.Data.CommonUserId.ToString()),
                    new Claim("RoleID", response.Data.RoleId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { AllowRefresh = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Dashboard"); // Redirect to secure page
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Email or Password";
                return View();
            }
        }
    }
}
