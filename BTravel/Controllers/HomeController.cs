using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.BL.Services.Contracts.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Mail;
using BTravel.BL.Services.Security.Auth;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Auth;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using BTravel.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;

namespace BTravel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BTravelDbContext _context;

        public HomeController(ILogger<HomeController> logger, BTravelDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Services()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Packages()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult FAQ()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ContactUs()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            var UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID"));

            var model = new ChangePasswordDTO
            {
                CommonUserId = UserID,
            };

            return View("~/Views/Home/ChangePassword.cshtml", model);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordDTO model)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new ChangePassword(request);
            var response = query.Change(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Contracts", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return View("~/Views/Home/ChangePassword.cshtml", model);
            }
        }

        [HttpGet]
        public IActionResult TestEmail()
        {           
            //return View("~/Views/Home/TestEmail.cshtml");
            return NotFound();
        }

        [HttpPost]
        public IActionResult TestEmail(string EmailAddress)
        {
            try
            {
                //MailSender.SendMail(EmailAddress, "Test from BTravelMate", "Test from BTravelMate server");

                TempData["SuccessMessage"] = "Email sent successfully";
                return View("~/Views/Home/TestEmail.cshtml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error : {ex.Message}";
                return View("~/Views/Home/TestEmail.cshtml");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}