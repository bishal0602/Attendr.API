using Attendr.API.Extensions;
using Serilog;

namespace Attendr.API
{
    internal static class ServiceExtensions
    {
        // Add services to container
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            builder.Services.ConfigureCors();
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

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors();

            app.UseSerilogRequestLogging();

            return app;
        }
    }
}
