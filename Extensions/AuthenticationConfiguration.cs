using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Attendr.API.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "attendrapi";
                    options.TokenValidationParameters = new()
                    {
                        RoleClaimType = "role",
                        ValidTypes = new[] { "at+jwt" }
                    };
                });
            return services;
        }
    }
}
