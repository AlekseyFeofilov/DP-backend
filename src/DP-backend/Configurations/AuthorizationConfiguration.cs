using DP_backend.Controllers;
using DP_backend.Domain.Identity;

namespace DP_backend.Configurations
{
    public static class AuthorizationConfiguration
    {
        public static void ConfigureClaimAuthorization(this WebApplicationBuilder? builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.Services.AddAuthorization(options =>
            {


                options.AddPolicy("GroupControl", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(ApplicationRoleNames.Administrator, ApplicationRoleNames.Staff);
                });

                options.AddPolicy("UserControl", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(ApplicationRoleNames.Administrator, ApplicationRoleNames.Staff);
                });
                options.AddPolicy("EmployerControl", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(ApplicationRoleNames.Administrator, ApplicationRoleNames.Staff);
                });
                options.AddPolicy("EmploymentControl", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(ApplicationRoleNames.Student, ApplicationRoleNames.Staff, ApplicationRoleNames.Administrator);
                });

                options.AddPolicy("EmploymentDelete", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(ApplicationRoleNames.Staff, ApplicationRoleNames.Administrator);
                });

                options.AddPolicy(ApplicationRoleNames.Student, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(ApplicationRoleNames.Student);
                });


            });

        }
    }
}
