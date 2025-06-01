using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BTravel.DAL;
using BTravel.CommonDefinitions.Enums;
using BTravel.DAL.Entities;
using Microsoft.EntityFrameworkCore;

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
        private readonly IServiceProvider _serviceProvider;

        public AuthorizePerRoleFilter(string serviceName, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this._serviceName = serviceName;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var response = new BaseResponse<bool>();
            //response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            //response.Success = false;
            //response.Data = false;
            //response.Message = "Unauthorized Access";

            //string connectionString = _configuration.GetConnectionString("BTravelConnection");

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                bool canAccess = false;

                var RoleID = int.Parse(context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleID").Value);

                using (var scope = _serviceProvider.CreateScope())
                {
                    //*** Create a new fresh instance from db context
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var connectionString = configuration.GetConnectionString("BTravelConnection");

                    var optionsBuilder = new DbContextOptionsBuilder<BTravelDbContext>();
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                    using var freshDbContext = new BTravelDbContext(optionsBuilder.Options);

                    if (RoleID == 0)
                        canAccess = false;

                    if (RoleID == (long)ERole.Admin)
                        canAccess = true;
                    else
                    {
                        var service = freshDbContext.AppServices.FirstOrDefault(c => !c.IsDeleted && c.Name.ToLower() == _serviceName.ToLower());
                        if (service == null)
                            canAccess = false;

                        canAccess = freshDbContext.RoleAppServices.Any(c => c.RoleId == RoleID && c.AppServiceId == service.AppServiceId && c.IsActive && !c.IsDeleted);
                    }
                }

                if (canAccess == false)
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