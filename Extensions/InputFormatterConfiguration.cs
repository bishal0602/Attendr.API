using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Attendr.API.Extensions
{
    public static class InputFormatterConfiguration
    {
        public static IMvcBuilder ConfigureInputFormatter(this IMvcBuilder builder)
        {

            return builder.AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
             })
                    .ConfigureApiBehaviorOptions(setupAction =>
                    {
                        setupAction.InvalidModelStateResponseFactory = context =>
                        {

                            var problemDetailsFactory = context.HttpContext.RequestServices
                                .GetRequiredService<ProblemDetailsFactory>();

                            var validationProblemDetails = problemDetailsFactory
                                .CreateValidationProblemDetails(
                                    context.HttpContext,
                                    context.ModelState);

                            validationProblemDetails.Detail =
                                "See the errors field for details.";
                            validationProblemDetails.Instance =
                                context.HttpContext.Request.Path;
                            validationProblemDetails.Status =
                                StatusCodes.Status422UnprocessableEntity;
                            validationProblemDetails.Title =
                                "One or more validation errors occurred.";

                            return new UnprocessableEntityObjectResult(
                                validationProblemDetails)
                            {
                                ContentTypes = { "application/problem+json" }
                            };
                        };
                    });
        }
    }
}
