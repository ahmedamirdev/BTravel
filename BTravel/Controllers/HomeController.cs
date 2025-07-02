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
using BTravel.BL.Services.Contracts.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Mail;
using BTravel.BL.Services.Security.Auth;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Auth;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
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
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [AuthorizePerRole("View_Contract_Portal")]
        public IActionResult Contracts(int PageIndex = 0, string Search = "")
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
                PageIndex = PageIndex,
            };

            var query = new GetAllContractsByCustomerId(request);
            var response = query.GetAll(Search, request.UserID);

            return View("~/Views/Home/Contracts.cshtml", response);
        }

        [AuthorizePerRole("View_Contract_Portal")]
        public IActionResult CDetails(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForCustomer(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Home/CDetails.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Contracts", "Home");
            }
        }

        [AuthorizePerRole("Sign_Contract_Portal")]
        public IActionResult CSign(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForSign(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //TempData["SuccessMessage"] = response.Message;
                return View("~/Views/Home/CSign.cshtml", response.Data);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Contracts", "Home");
            }
        }

        [HttpPost]
        [AuthorizePerRole("Sign_Contract_Portal")]
        public IActionResult SignContract(ContractDTO model)
        {
            #region Create Signature Image

            if (string.IsNullOrWhiteSpace(model.signatureData))
            {
                TempData["ErrorMessage"] = "Signature is empty";
                return RedirectToAction("CSign", "Home", new { Id = model.ContractId });
            }

            // Remove base64 header
            var base64Data = model.signatureData.Split(',')[1];
            var imageBytes = Convert.FromBase64String(base64Data);

            // Create a MemoryStream from the byte array
            using (var stream = new MemoryStream(imageBytes))
            {
                // Create a form file from the stream
                IFormFile formFile = new FormFile(stream, 0, stream.Length, "signature", "signature.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var file = formFile.OpenReadStream();
                var fileName = formFile.FileName;
                if (file.Length > 0)
                {
                    var newFileName = Guid.NewGuid().ToString() + "-" + fileName;
                    var physicalPath = Directory.GetCurrentDirectory() + "/wwwroot/" + "Content/" + newFileName;
                    string dirPath = Path.GetDirectoryName(physicalPath);

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    var virtualPath = "Content/" + newFileName;

                    using (var streamFile = new FileStream(physicalPath, FileMode.Create))
                    {
                        file.CopyTo(streamFile);
                    }

                    model.SignatureUrl = virtualPath;
                }
            }

            #endregion

            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new SignContract(request);
            var response = query.Sign(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("CDetails", "Home", new { Id = model.ContractId });
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("CSign", "Home", new { Id = model.ContractId });
            }
        }

        [AuthorizePerRole("Download_Contract_Portal")]
        public async Task<IActionResult> DownloadContract(int Id)
        {
            var request = new BaseRequest
            {
                Context = _context,
                RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID")),
            };

            var query = new GetContractByIdForCustomer(request);
            var response = query.GetById(Id);

            if (response.Success)
            {
                //*** Choice (1)

                //// Path to your HTML file
                //var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "Home", "ContractPDF.cshtml");

                //// Read the HTML content
                //var htmlContent = System.IO.File.ReadAllText(htmlFilePath);

                //// Create a PDF document
                //PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent, PdfSharp.PageSize.A4);

                //// Save the PDF to a MemoryStream
                //using (var stream = new MemoryStream())
                //{
                //    pdf.Save(stream, false);
                //    var pdfBytes = stream.ToArray();

                //    // Return the PDF file for download
                //    return File(pdfBytes, "application/pdf", "SampleDocument.pdf");
                //}


                //*** Choice (2)

                //string htmlContent = await _viewRenderService.RenderToStringAsync("ContractPDF", response.Data);

                //PdfSharp.PageSize s = PdfSharp.PageSize.A4;
                //var pdf = PdfGenerator.GeneratePdf(htmlContent, s);
                //using var stream = new MemoryStream();
                //pdf.Save(stream, false);
                //stream.Position = 0;

                //return File(stream.ToArray(), "application/pdf", "Contract.pdf");


                //*** Choice (3)

                //return new ViewAsPdf("ContractPDF", response.Data)
                //{
                //    FileName = $"Contract_{response.Data.ContractId}.pdf",
                //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                //};

                return null;
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Contracts", "Home");
            }
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
    }

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