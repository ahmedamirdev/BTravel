using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using BTravel.Services.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BTravel.Services.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        //*** Configure Global Exception Handler
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        int userID = 0;
                        // updaed code to handle null userID
                        if (context.User.Identity.IsAuthenticated)
                        {
                            var userIdClaim = context.User.FindFirst("UserID");
                            if (userIdClaim != null)
                            {
                                userID = int.Parse(userIdClaim.Value);
                            }
                        }

                        string errorMessage = $" \n\n >>>>>>>>>>>>>>>>>>>>>>>>> Internal Server Error @ {DateTime.UtcNow} - UserID #{userID} <<<<<<<<<<<<<<<<<<<<<<<<< \n\n";
                        errorMessage += $" {contextFeature.Error} \n\n";
                        logger.LogError(errorMessage);

                        //await context.Response.WriteAsync(new ErrorDTO()
                        //{
                        //    StatusCode = context.Response.StatusCode,
                        //    Message = $"Internal Server Error : {contextFeature.Error.Message}",
                        //}.ToString());
                    }
                });
            });
        }
    }

    public class ErrorDTO
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}