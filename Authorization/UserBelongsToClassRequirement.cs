using Microsoft.AspNetCore.Authorization;

namespace Attendr.API.Authorization
{
    public class UserBelongsToClassRequirement : IAuthorizationRequirement
    {
        public UserBelongsToClassRequirement()
        {

        }
    }
}
