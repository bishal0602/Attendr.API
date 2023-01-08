using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Attendr.API.Authorization
{
    public class UserBelongsToClassHandler : AuthorizationHandler<UserBelongsToClassRequirement>
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public UserBelongsToClassHandler(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        //}
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserBelongsToClassRequirement requirement)
        {
            // TO GET SOEMTHING FROM ROUTE
            // _httpContextAccessor.HttpContext?.GetRouteValue("PARAMETER")?.ToString();

            // TO GET SOMETHING FROM CLAIMS
            // context.User.Claims.FirstOrDefault(c => c.Type == "CLAIMTYPE")?.Value;

            // IF SOMETHING FAILS
            // context.Fail();
            // return;



            var identity = context.User.Identity;
            if (identity is null)
            {
                throw new Exception("Identity Error!");
            }
            var claims = ((ClaimsIdentity)identity).Claims;
            var roles = claims.FirstOrDefault(c => c.Type == "role")?.Value;
            if (roles is null)
            {
                throw new ArgumentException("Role claims not provided");
            }
            if (!roles.Contains("cr"))
            {
                context.Fail();

            }
            await Task.Delay(500);
            // all checks out
            context.Succeed(requirement);
        }
    }
}
