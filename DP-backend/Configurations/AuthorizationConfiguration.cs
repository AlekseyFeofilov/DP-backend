using DP_backend.Models;

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

                options.AddPolicy(ApplicationRoleNames.Administrator, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ApplicationRoleNames.Administrator, "true");
                });
                options.AddPolicy(ApplicationRoleNames.Student, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ApplicationRoleNames.Student, "true");
                });

                options.AddPolicy(ApplicationRoleNames.Staff, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ApplicationRoleNames.Staff, "true");
                });

                options.AddPolicy(ApplicationRoleNames.NoOne, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ApplicationRoleNames.NoOne, "true");
                });
            });

        }
    }
}
