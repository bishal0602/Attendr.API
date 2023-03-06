using Serilog;
using Attendr.API.DbContexts;
using Attendr.API.Extensions;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Attendr.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Attendr.API
{
    internal static class ServiceExtensions
    {
        // Add services to container
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();

            builder.Services.AddEndpointsApiExplorer()
                            .ConfigureSwagger();

            builder.Services.ConfigureControllers()
                            .ConfigureInputFormatter();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.ConfigureIdentityServer()
                            .ConfigureAuthorization();

            builder.Services.ConfigureServices()
                            .ConfigureHelpers()
                            .AddAutoMapper(typeof(Program))
                            .AddHttpContextAccessor()
                            .ConfigureOutputFormatter()
                            .ConfigureCors();

            builder.Services.AddDbContext<AttendrAPIDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:AttendrAPIDB"]!);
            });

            return builder.Build();
        }

        // Configure the HTTP request pipeline.
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(setupAction =>
                {
                    setupAction.RoutePrefix = string.Empty;
                    setupAction.SwaggerEndpoint("/swagger/AttendrAPIOpenAPISpecification/swagger.json", "Attendr API");
                });
            }
            app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseCors();

            app.UseSerilogRequestLogging();

            return app;
        }
    }
}
