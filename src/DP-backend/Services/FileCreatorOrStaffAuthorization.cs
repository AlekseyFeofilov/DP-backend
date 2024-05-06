using DP_backend.Database;
using DP_backend.Domain.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DP_backend.Services;

public record FileCreatorOrStaff(Guid FileId) : IAuthorizationRequirement;

public class FileCreatorOrStaffAuthorization : AuthorizationHandler<FileCreatorOrStaff>
{
    private readonly ApplicationDbContext _dbContext;

    public FileCreatorOrStaffAuthorization(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FileCreatorOrStaff requirement)
    {
        if (context.User.IsInRole(ApplicationRoleNames.Staff) || context.User.IsInRole(ApplicationRoleNames.Administrator))
        {
            context.Succeed(requirement);
            return;
        }

        var fileHandle = await _dbContext.FileHandles.FindAsync([ requirement.FileId ]);
        if (fileHandle?.Id == requirement.FileId)
        {
            context.Succeed(requirement);
        }
    }
}