using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BTravel.BL.Services.Security.Auth
{
    public class AuthorizePerRoleAttribute : TypeFilterAttribute
    {
        public AuthorizePerRoleAttribute(string serviceName) : base(typeof(AuthorizePerRoleFilter))
        {
            Arguments = new object[] { serviceName };
        }
    }

    public class AuthorizePerRoleFilter : IAuthorizationFilter
    {
        readonly string _serviceName;
        private readonly IConfiguration _configuration;

        public AuthorizePerRoleFilter(string serviceName, IConfiguration configuration)
        {
            this._serviceName = serviceName;
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var response = new BaseResponse<bool>();
            //response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            //response.Success = false;
            //response.Data = false;
            //response.Message = "Unauthorized Access";

            string connectionString = _configuration.GetConnectionString("BTravelConnection");

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var RoleID = int.Parse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

                bool canAccess = BaseService.CheckRoleAccessability(RoleID, _serviceName, connectionString: connectionString);
                if (!canAccess)
                {
                    //context.Result = new UnauthorizedObjectResult(response);
                    context.Result = new UnauthorizedResult();
                }
            }
            else
                context.Result = new UnauthorizedResult();
        }
    }
}