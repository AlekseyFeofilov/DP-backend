using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Models.DTOs.TSUAccounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IUserManagementService
    {
        public Task<User> GetUserByAccountId(Guid accountId);
        public Task<User> CreateUserByAccountId(Guid accountId, bool asStudent);
        public Task ChangeUserRole(Guid userId, ApplicationRoles roleName);
        public Task SetStudentGroup(Guid userId, Guid groupId);
        public Task<List<StudentDTO>> GetStudentsFromGroup (Guid? groupId, bool withoutGroup);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly ITSUAccountService _accountService;
        private readonly ITSUAccountService _tsuAccountService;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;


        public UserManagementService(ITSUAccountService tSUAccountService, ApplicationDbContext context, UserManager<User> userManager, ITSUAccountService tsuAccountService)
        {
            _accountService = tSUAccountService;
            _dbContext = context;
            _userManager = userManager;
            _tsuAccountService = tsuAccountService;
        }

        public async Task<User> GetUserByAccountId(Guid accountId)
        {
            return await _dbContext.Users.GetUndeleted().FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<User> CreateUserByAccountId(Guid accountId, bool asStudent)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.AccountId == accountId);
            if (user != null)
            {
                throw new ArgumentException("Invalid accountId");
            }

            user = await GetUserFromTsuAccounts(accountId, new User());
            if (user == null)
            {
                return null;
            }

            _dbContext.Users.Add(user);


            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }

            try
            {
                if (asStudent)
                {
                    await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Student);
                    _dbContext.Students.Add(new Student { UserId = user.Id });
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, ApplicationRoleNames.NoOne);
                }
            }
            catch
            {
                throw;
            }

            return user;
        }

        private async Task<User> GetUserFromTsuAccounts(Guid accountId, User user)
        {
            TSUAccountsUserModelDTO tsuAccountUserModel;
            try
            {
                tsuAccountUserModel = await _tsuAccountService.GetUserModelByAccountId(accountId);
                if (tsuAccountUserModel == null)
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            user.UserName = tsuAccountUserModel.FullName;
            user.EmailConfirmed = true;
            user.PhoneNumber = tsuAccountUserModel.Phone;
            user.PhoneNumberConfirmed = true;
            user.AccountId = accountId;
            if (_tsuAccountService.IsValidTsuAccountEmail(tsuAccountUserModel.Email))
            {
                user.UserName = tsuAccountUserModel.Email;
                user.Email = tsuAccountUserModel.Email;
            }
            else
            {
                user.UserName = accountId.ToString();
            }

            return user;
        }

        public async Task ChangeUserRole(Guid userId, ApplicationRoles roleName)
        {
            var user = await _dbContext.Users.Include(x => x.Roles).ThenInclude(x => x.Role).GetUndeleted().FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"There is no user with {userId} Id!");
            }
            try
            {
                await _userManager.RemoveFromRolesAsync(user, user.Roles.Select(x => x.Role.Name));
                await _userManager.AddToRoleAsync(user, ApplicationRoleNames.SystemRoleNamesDictionary[roleName]);

                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task SetStudentGroup(Guid userId, Guid groupId)
        {
            var student = await _dbContext.Students.Include(x=>x.Group).FirstOrDefaultAsync(x => x.UserId == userId);
            if (student == null)
            {
                throw new KeyNotFoundException($"There is no student with this {userId} id!");
            }
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x=>x.Id==groupId);
            if (group == null) 
            {
                throw new KeyNotFoundException($"There is no group with this {groupId} id!");
            }
            student.Group= group;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<StudentDTO>> GetStudentsFromGroup(Guid? groupId, bool withoutGroup)
        {
            return await _dbContext.Students
                .Where(x=> withoutGroup? (x.GroupId==null) : (groupId==null? true : x.GroupId==groupId))
                .Select(x=> new StudentDTO(x)).ToListAsync();
        }
    }
}