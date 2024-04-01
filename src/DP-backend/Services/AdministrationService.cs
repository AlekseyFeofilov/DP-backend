using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IAdministrationService
    {
        public Task ChangeUserRole(Guid userId, ApplicationRoles roleName);
    }

    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManger;
        public AdministrationService(
            ApplicationDbContext context,
            UserManager<User> userManger
            )
        {
            _context = context;
            _userManger = userManger;
        }
        public async Task ChangeUserRole(Guid userId, ApplicationRoles roleName)
        {
            var user = await _context.Users.Include(x=>x.Roles).ThenInclude(x => x.Role).GetUndeleted().FirstOrDefaultAsync(x=>x.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"There is no user with {userId} Id!");
            }
            try
            {
                await _userManger.RemoveFromRolesAsync(user, user.Roles.Select(x => x.Role.Name));
                await _userManger.AddToRoleAsync(user, ApplicationRoleNames.SystemRoleNamesDictionary[roleName]);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
