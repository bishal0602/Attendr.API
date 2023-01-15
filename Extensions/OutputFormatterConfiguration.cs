using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace Attendr.API.Extensions
{
    public static class OutputFormatterConfiguration
    {
        public static void ConfigureOutputFormatter(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(configureOptions =>
            {
                var jsonOutputFormatter = configureOptions.OutputFormatters
                    .OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    // remove text/json as it isn't the approved media type for working with JSON at API level
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            });
        }
    }
}
