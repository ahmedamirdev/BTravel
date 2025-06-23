using System.Text;
using System.Threading.Tasks;
using BTravel.Controllers;
using BTravel.DAL;
using BTravel.Services.Extensions;
using BTravel.Services.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Rotativa.AspNetCore;

namespace BTravel
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //***************************************** Create the WebApplication *****************************************//
            var builder = WebApplication.CreateBuilder(args);

            //*** Add Logger Configuration --- nlog library
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            builder.Services.AddSingleton<ILoggerService, LoggerService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //*** Add BTravel dbContext
            var dbConnectionString = builder.Configuration.GetValue<string>("ConnectionStrings:BTravelConnection");
            builder.Services.AddDbContext<BTravelDbContext>(opt => opt.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));

            //*** Add Authentication Scheme
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.LoginPath = "/Dashboard/Login"; // Redirect to login if not authenticated
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

            builder.Services.AddControllers();


            //***************************************** Build the WebApplication *****************************************//
            var app = builder.Build();

            //*** Register exception handler HERE in the early stage of the pipeline flow ..........
            var logger = app.Services.GetRequiredService<ILoggerService>();
            //app.ConfigureExceptionHandler(logger);

            RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
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

            // app.UseEndpoints(e => e.MapControllers());

            //*** Add Custom Middleware
            //app.UseMiddleware<MyCustomMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            try
            {
                using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<BTravelDbContext>();
                    context.Database.Migrate();
                    DatabaseSeeder.SeedRolesAndServices(context);
                }
            }
            catch { }

            app.Run();
        }
    }
}