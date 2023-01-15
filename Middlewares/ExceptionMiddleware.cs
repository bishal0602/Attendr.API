using Attendr.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System;
using System.Net;

namespace Attendr.API.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await httpContext.Response.WriteAsJsonAsync(new ErrorDetails(StatusCodes.Status401Unauthorized, "Authenticated failed", "Provide a valid token."));
                }
                if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await httpContext.Response.WriteAsJsonAsync(new ErrorDetails(StatusCodes.Status403Forbidden, "Forbidden rescource", "You are not permitted to access this rescource."));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var message = exception switch
            {
                _ => "Internal Server Error"
            };

            await context.Response.WriteAsJsonAsync(new ErrorDetails(context.Response.StatusCode, message, exception.Message));
        }
    }
}
