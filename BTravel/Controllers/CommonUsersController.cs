using BTravel.BL.Services.CommonUser.Commands;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Requests;
using BTravel.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTravel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonUsersController : ControllerBase
    {
        private readonly BTravelDbContext _dbContext;

        public CommonUsersController(BTravelDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        //[Route("/api/users/search")]
        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string term)
        {
            var results = await _dbContext.CommonUsers
                .Where(c => c.FullName.Contains(term) || c.FullName.Contains(term))
                .Select(c => new { id = c.CommonUserId, name = c.FullName })
                .Take(20) // limit for performance
                .ToListAsync();

           


            var baseRequest = new BaseRequest();
            baseRequest.Context = _dbContext;

            var user = new CommonUserAddDTO
            {
                PrimaryMail = "ahmed_amirr@hotmail.com",
                FullName = "Ahmed Amir",
                PhoneNumber = "01061605524",
                CompanyName = "Test",
                CardNumber = "12345678",
                CardExpDate = "123",
                CardCVC = "123",
                NameOnCreditCard = "Ahmed Amir",
                BillingAddress = "Ahmed Amir",
                PostalCode = "12345",
                Password = "123",
                RoleId = 1,
            };

            var user2 = new CommonUserAddDTO
            {
                PrimaryMail = "safeyz7@hotmail.com",
                FullName = "Mustafa Safey",
                PhoneNumber = "01061605524",
                CompanyName = "Test",
                CardNumber = "12345678",
                CardExpDate = "123",
                CardCVC = "123",
                NameOnCreditCard = "Mustafa Safey",
                BillingAddress = "Mustafa Safey",
                PostalCode = "12345",
                Password = "123",
                RoleId = 1,
            };

            var user3 = new CommonUserAddDTO
            {
                PrimaryMail = "mahmoud@hotmail.com",
                FullName = "Mahmoud Men3m",
                PhoneNumber = "01061605524",
                CompanyName = "Test",
                CardNumber = "12345678",
                CardExpDate = "123",
                CardCVC = "123",
                NameOnCreditCard = "Mahmoud Men3m",
                BillingAddress = "Mahmoud Men3m",
                PostalCode = "12345",
                Password = "123",
                RoleId = 1,
            };

            var query = new AddCommonUser(baseRequest);
            var result = query.Add(user);
            result = query.Add(user2);
            result = query.Add(user3);

            return Ok(results);
        }
    }
}