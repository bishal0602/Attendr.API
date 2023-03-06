using Attendr.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

namespace Attendr.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
