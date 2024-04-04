namespace DP_backend.Services.Initialization;

public static class ServicesInitializer
{
    public static void InitInternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<ITSUAccountService, TSUAccountService>();
        services.AddScoped<IAdministrationService, AdministrationService>();
        services.AddScoped<IEmployerService, EmployerService>();
        services.AddScoped<IEmploymentVariantService, EmploymentVariantService>();
        services.AddScoped<IEmploymentService, EmploymentService>();
    }
}