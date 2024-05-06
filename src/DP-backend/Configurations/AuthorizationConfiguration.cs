using DP_backend.Domain.Identity;

namespace DP_backend.Configurations;

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
            options.AddPolicy(ApplicationRoleNames.Student, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(ApplicationRoleNames.Student);
            });

            options.AddPolicy("StaffAndStudent", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(ApplicationRoleNames.Student, ApplicationRoleNames.Staff, ApplicationRoleNames.Administrator);
            });
            options.AddPolicy("Staff", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(ApplicationRoleNames.Staff, ApplicationRoleNames.Administrator);
            });
        });

    }
}