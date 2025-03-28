using System.Text;
using BTravel.Controllers;
using BTravel.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BTravel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //***************************************** Create the WebApplication *****************************************//
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //*** Add BTravel dbContext
            var dbConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:BTravelConnection");
            builder.Services.AddDbContext<BTravelDbContext>(opt => opt.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));

            //*** Add Authentication Scheme
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.LoginPath = "/Home/TestLogin"; // Redirect to login if not authenticated
                                options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if access denied
                            });

            //*** Add Authentication Scheme with JWT Bearer
            //builder.Services.AddAuthentication(options =>
            //                {
            //                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            //                })
            //                .AddCookie(options =>
            //                {
            //                    options.LoginPath = "/Home/TestLogin"; // Redirect to login if not authenticated
            //                    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if access denied
            //                })
            //                .AddJwtBearer(options =>
            //                {
            //                    options.SaveToken = true;
            //                    options.RequireHttpsMetadata = false;
            //                    options.TokenValidationParameters = new TokenValidationParameters()
            //                    {
            //                        ValidateIssuer = true,
            //                        ValidateAudience = true,
            //                        ValidAudience = "BTravelMATE",
            //                        ValidIssuer = "BTravelMATE",
            //                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BTravelMATENowSecurityKey_Asgiusdf64nf#kjd&g5"))
            //                    };
            //                });

            //*** Add CORS Policy
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            builder.Services.AddSession();

            builder.Services.AddMvc().AddSessionStateTempDataProvider();



            //***************************************** Build the WebApplication *****************************************//
            var app = builder.Build();

            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("MyPolicy");

            //app.UseMvc();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //*** Add Custom Middleware
            //app.UseMiddleware<MyCustomMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}