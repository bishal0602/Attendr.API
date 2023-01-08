using Attendr.API.Extensions;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace Attendr.API
{
    internal static class ServiceExtensions
    {
        // Add services to container
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();
            builder.Services.ConfigureCors();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.ConfigureIdentityServer();
            builder.Services.ConfigureAuthorization();

            builder.Services.AddAutoMapper(typeof(Program));

            return builder.Build();
        }

        // Configure the HTTP request pipeline.
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
