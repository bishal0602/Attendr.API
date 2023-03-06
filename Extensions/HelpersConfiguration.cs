using Attendr.API.Helpers;

namespace Attendr.API.Extensions
{
    public static class HelpersConfiguration
    {
        public static IServiceCollection ConfigureHelpers(this IServiceCollection services)
        {
            services.AddScoped<IClassStudentHelper, ClassStudentHelper>();
            services.AddScoped<IIdentityHelper, IdentityHelper>();
            return services;
        }
    }
}
