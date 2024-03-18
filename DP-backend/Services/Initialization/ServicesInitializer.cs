using DP_backend.Services;

namespace DP_backend.Services.Initialization
{
    public static class ServicesInitializer
    {
        public static void InitInternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();
            services.AddScoped<ITSUAccountService, ITSUAccountService>();
        }
    }
}
