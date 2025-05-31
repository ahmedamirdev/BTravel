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
        private readonly BTravelDbContext _context;

        public CommonUsersController(BTravelDbContext context)
        {
            _context = context;
        }

        //[Route("/api/users/search")]
        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string term)
        {
            var results = await _context.CommonUsers
                .Where(c => !c.IsDeleted && c.IsActive)
                .Where(c => c.FullName.Contains(term) || c.PrimaryMail.Contains(term) || c.PhoneNumber.Contains(term))
                .Select(c => new { id = c.CommonUserId, name = c.FullName + " (" + c.PhoneNumber + ")" })
                .ToListAsync();

            return Ok(results);
        }
    }
}