using Attendr.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
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
