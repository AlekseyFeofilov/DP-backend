using DP_backend.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace DP_backend.Services.Initialization
{
    public static class RoleInitializer
    {
        public static async Task Initialize(this IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var userManagementService = scope.ServiceProvider.GetRequiredService<IUserManagementService>();
                var allRoles = roleManager.Roles.ToList();
                var currentTime = DateTime.Now;
                foreach (var roleType in Enum.GetValues<ApplicationRoles>())
                {
                    var role = allRoles.FirstOrDefault(x => x.Name == roleType.ToString());
                    if (role == null)
                    {
                        role = new Role
                        {
                            Name = roleType.ToString(),
                            CreateDateTime = currentTime,
                            ModifyDateTime = currentTime
                        };
                        await roleManager.CreateAsync(role);
                    }
                }
                await InitializeAdministrators(userManager, configuration, userManagementService);
            }
        }

        private static async Task InitializeAdministrators(UserManager<User> userManager, IConfiguration configuration,
            IUserManagementService userManagementService)
        {
            var accountIds = configuration.GetSection("Administrators").Get<List<Guid>>() ?? new List<Guid>();
            foreach (var accountId in accountIds)
            {
                await InitializeAdministrator(userManager, userManagementService, accountId);
            }
        }

        private static async Task InitializeAdministrator(UserManager<User> userManager, IUserManagementService userManagementService, Guid accountId)
        {
            var administrator = await userManagementService.GetUserByAccountId(accountId) ??
                                await userManagementService.CreateUserByAccountId(accountId, true);

            if (!await userManager.IsInRoleAsync(administrator, ApplicationRoles.Administrator.ToString()))
            {
                await userManagementService.ChangeUserRole(administrator.Id,ApplicationRoles.Administrator);
            }
        }
    }
}
