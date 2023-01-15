using Attendr.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendr.API.Extensions
{
    public static class ControllersConfiguration
    {
        public static IMvcBuilder ConfigureControllers(this IServiceCollection services)
        {
            return services.AddControllers(configure =>
             {
                 configure.ReturnHttpNotAcceptable = true;

                 configure.Filters.Add(new ProducesAttribute("application/json"));
                 configure.Filters.Add(new ConsumesAttribute("application/json"));

                 configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                 configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status415UnsupportedMediaType));
                 configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetails), StatusCodes.Status401Unauthorized));
                 configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetails), StatusCodes.Status403Forbidden));
                 configure.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetails), StatusCodes.Status500InternalServerError));

             });
        }
    }
}
