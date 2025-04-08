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

namespace BTravel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BTravelDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, BTravelDbContext dbcontext)
        {
            _logger = logger;
            _dbContext = dbcontext;
        }

        public IActionResult Index()

        {
            List<Contract> data = new List<Contract>();

            data.Add(new Contract
            {
                ContractId = 1,
                HotelName = "shiraton",

            });
            data.Add(new Contract
            {
                ContractId = 2,
                HotelName = "sofitel",

            });
            data.Add(new Contract
            {
                ContractId = 3,
                HotelName = "helton",

            });


            return View(data);
        }

        public IActionResult Test()
        {
            return View();
        }

        [Authorize]
        //[AuthorizePerRole("View_Avatar")]
        public IActionResult MyHomeView()
        {
            int RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID"));
            int UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID"));



            //string originalText = "123-456-789 ENG&1";
            //Console.WriteLine("Original Text: " + originalText);

            //string encryptedText = AESEncryptionHelper.Encrypt(originalText);
            //Console.WriteLine("Encrypted Text: " + encryptedText);

            //string decryptedText = AESEncryptionHelper.Decrypt(encryptedText);
            //Console.WriteLine("Decrypted Text: " + decryptedText);


            //string mailBody = $"Hello Ahmed,";
            //mailBody += $"<br><br> Your email has been verified successfully.";
            //mailBody += $"<br><br> Thanks for using BTravelMATE.";

            //MailSender.SendMail("ahmed_amirr@hotmail.com", $"Test Email Verified Successfully", mailBody);


            return View();
        }

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginDTO model)
        {
            var request = new BaseRequest();
            request.Context = _dbContext;

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

                return RedirectToAction("MyHomeView", "Home"); // Redirect to secure page
            }
            else
            {
                ViewBag["ErrorMessage"] = "Invalid Email or Password";
                return View();
            }
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}