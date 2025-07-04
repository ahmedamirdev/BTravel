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

using PdfSharp.Fonts;
//using TheArtOfDev.HtmlRenderer.PdfSharp;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf;
//using Rotativa.AspNetCore;
//using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace BTravel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BTravelDbContext _context;
        private readonly IViewRenderService _viewRenderService;

        public HomeController(ILogger<HomeController> logger, BTravelDbContext context, IViewRenderService viewRenderService)
        {
            _logger = logger;
            _context = context;
            _viewRenderService = viewRenderService;
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

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }



    //*** HTML to PDF using HTMLRender library
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }

    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
                throw new ArgumentNullException($"View '{viewName}' not found.");

            await using var sw = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }

    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            var fontPath = faceName switch
            {
                "Segoe UI" => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Fonts", "segoeui.ttf"),
                _ => throw new InvalidOperationException($"Font {faceName} not found.")
            };

            return File.ReadAllBytes(fontPath);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Segoe UI", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Segoe UI");
            }

            return null;
        }
    }
}