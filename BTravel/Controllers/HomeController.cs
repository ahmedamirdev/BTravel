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
            //if (User.Identity.IsAuthenticated)
            //{


            //    int RoleID = int.Parse(AuthHelper.GetClaimValue(User, "RoleID"));
            //    int UserID = int.Parse(AuthHelper.GetClaimValue(User, "UserID"));

            //    var x = User.Claims.First(c => c.Type == "RoleID");
            //    var x2 = User.Claims.First(c => c.Type == "RoleID").Value;

            //    var x3 = User.Claims.First(c => c.Type == "UserID");
            //    var x4 = User.Claims.First(c => c.Type == "UserID").Value;


            //    var y = User.Identity.IsAuthenticated;
            //    var z = User.Identity.AuthenticationType;
            //}

            return View();
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
    }
}