using Attendr.API.Services;

namespace Attendr.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IRoutineRepository, RoutineRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        }
    }
}
