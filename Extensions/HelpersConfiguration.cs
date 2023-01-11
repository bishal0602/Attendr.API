using Attendr.API.Helpers;

namespace Attendr.API.Extensions
{
    public static class HelpersConfiguration
    {
        public static void ConfigureHelpers(this IServiceCollection services)
        {
            services.AddScoped<IClassStudentHelper, ClassStudentHelper>();
            services.AddScoped<IIdentityHelper, IdentityHelper>();
        }
    }
}
