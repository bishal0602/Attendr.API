﻿using Attendr.API.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Attendr.API.Extensions
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.ConfigureAuthorzationHandlers();

            services.AddAuthorization(authorizationOptions =>
            {
                //authorizationOptions.AddPolicy("UserCanTakeAttendance", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.AddRequirements(
                //        new UserBelongsToClassRequirement()
                //        );
                //});
            });
            return services;
        }

        private static void ConfigureAuthorzationHandlers(this IServiceCollection services)
        {
            //services.AddScoped<IAuthorizationHandler, UserBelongsToClassHandler>();
        }
    }
}
