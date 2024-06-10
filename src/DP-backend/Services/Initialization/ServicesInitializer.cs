using DP_backend.Common.EntityType;
using Microsoft.AspNetCore.Authorization;

namespace DP_backend.Services.Initialization;

public static class ServicesInitializer
{
    public static void InitInternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<ITSUAccountService, TSUAccountService>();
        services.AddScoped<IEmployerService, EmployerService>();
        services.AddScoped<IEmploymentVariantService, EmploymentVariantService>();
        services.AddScoped<IEmploymentService, EmploymentService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IAuthorizationHandler, FileCreatorOrStaffAuthorization>();
        services.AddScoped<IInternshipDiaryRequestService, InternshipDiaryRequestService>();
        services.AddScoped<ICourseWorkRequestService, CourseWorkRequestService>();
        services.AddScoped<IEntityTypesProvider, DictionaryService>();
        services.AddScoped<IEnumDictionaryService, DictionaryService>();
    }
}