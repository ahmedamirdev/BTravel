using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BTravel.DAL.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Cryptography;
using BTravel.BL.Services.Security.Auth;
using BTravel.BL.Services.Security.Encryption;
using BTravel.BL.Services.Mail;
using System.Xml.Linq;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using BTravel.CommonDefinitions.DTOs.Auth;
using BTravel.BL.Services.Contracts.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics.Contracts;
using BTravel.BL.Services.Contracts.Commands;
using BTravel.CommonDefinitions.DTOs.Contract;
using Rotativa.AspNetCore;

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
        public IActionResult TermsAndCondetion()
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
        public IActionResult DownloadContract(int Id)
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
                return new ViewAsPdf("ContractPDF", response.Data)
                {
                    FileName = $"Contract_{response.Data.ContractId}.pdf",
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                };
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Contracts", "Home");
            }
        }
    }
}