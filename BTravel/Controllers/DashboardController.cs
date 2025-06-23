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
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.DTOs;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.DTOs.CommonUser;

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

        [AuthorizePerRole("View_Dashboard")]
        public IActionResult Index()
        {
            int x = int.Parse("ggg");

            var dto = new DashboardDTO();

            dto.TotalNumOfCustomers = _context.CommonUsers.Where(u => !u.IsDeleted).Count();

            dto.PendingContracts = _context.Contracts.Where(c => !c.IsDeleted && c.StatusId == (int)EContractStatus.Pending).Count();

            dto.OpenedContracts = _context.Contracts.Where(c => !c.IsDeleted && c.StatusId == (int)EContractStatus.Opened).Count();

            dto.SignedContracts = _context.Contracts.Where(c => !c.IsDeleted && c.StatusId == (int)EContractStatus.Sigend).Count();

            dto.TotalSalesAllTime = _context.Contracts.Where(c => !c.IsDeleted).Sum(c => c.Total);

            dto.FirstContractCreatedAt = _context.Contracts.Where(c => !c.IsDeleted).Min(c => c.CreatedAt);

            // Define the date range = get sales for last 7 days
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-7);

            dto.LastWeekStartAt = startDate;

            dto.TotalSalesLastWeek = _context.Contracts.Where(c => !c.IsDeleted)
                                                        .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                                                        .Sum(c => c.Total);

            dto.LatestContracts = _context.Contracts.Where(c => !c.IsDeleted)
                                                    .OrderByDescending(c => c.CreatedAt)
                                                    .Select(c => new ContractDTO
                                                    {
                                                        ContractId = c.ContractId,
                                                        CreatedAt = c.CreatedAt.AddHours(2),
                                                        StatusId = c.StatusId,
                                                        HotelName = c.HotelName,
                                                        Total = c.Total,

                                                        CommonUser = new CommonDefinitions.DTOs.CommonUser.CommonUserDTO
                                                        {
                                                            CommonUserId = c.CommonUserId,
                                                            FullName = c.CommonUser.FullName,
                                                        }
                                                    }).Take(10).ToList();

            dto.LatestUsers = _context.CommonUsers.Where(c => !c.IsDeleted)
                                                  .OrderByDescending(f => f.CreatedAt)
                                                  .Select(c => new CommonUserDTO
                                                  {
                                                      CommonUserId = c.CommonUserId,
                                                      FullName = c.FullName,
                                                      PhoneNumber = c.PhoneNumber,
                                                      PrimaryMail = c.PrimaryMail,
                                                      CreatedAt = c.CreatedAt.AddHours(2),
                                                      ImageUrl = c.ImageUrl,
                                                      RoleId = c.RoleId,
                                                  }).Take(5).ToList();

            return View(dto);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("LoginPost")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPost(LoginDTO model)
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
                    new Claim("RoleID", response.Data.RoleId.ToString()),
                    new Claim(ClaimTypes.Name, response.Data.FullName.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { AllowRefresh = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (response.Data.RoleId == (int)ERole.Admin)
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirect to secure page
                }
                else if (response.Data.RoleId == (int)ERole.Client)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to Home - non secure page
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Redirect to Home - non secure page
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Email or Password";
                return RedirectToAction("Login", "Dashboard");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Dashboard");
        }
    }
}