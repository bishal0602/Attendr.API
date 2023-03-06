using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Attendr.API.Extensions
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.AddSecurityDefinition("JWT Bearer Token",
                    new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Description = "Input your JWT Bearer Token to access the API"
                    });

                setupAction.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "JWT Bearer Token"
                                }
                            }, new List<string>()
                        }
                    }
                    );

                setupAction.SwaggerDoc("AttendrAPIOpenAPISpecification", new()
                {
                    Title = "Attendr API",
                    Version = "v1",
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(
                    AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
            return services;
        }
    }
}
