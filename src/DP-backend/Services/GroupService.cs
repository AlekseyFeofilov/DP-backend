using DP_backend.Common.Enumerations;
using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Models.DTOs;
using DP_backend.Models.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{

    public interface IGroupService
    {
        public Task CreateGroup(int groupNumber, Grade grade);
        public Task DeleteGroup(Guid groupId);
        public Task<List<GroupDTO>> GetGroups(Grade? grade);
        public Task ChangeGroupGrade(Guid groupId, Grade grade);
    
    }
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task ChangeGroupGrade(Guid groupId, Grade grade)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                throw new NotFoundException($"There is no group with this {groupId} id!");
            }
            group.Grade = grade;
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateGroup(int groupNumber, Grade grade)
        {
          var group =  await _dbContext.Groups.FirstOrDefaultAsync(x => x.Number == groupNumber);
            if (group != null)
            {
                throw new InvalidOperationException($"There is already a group with this {groupNumber} groupNumber!");
            }
            group = new Group
            {
                Id = Guid.NewGuid(),
                Grade = grade,
                Number = groupNumber
            };
            await _dbContext.Groups.AddAsync(group);   
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGroup(Guid groupId)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                throw new NotFoundException($"There is no group with this {groupId} id!");
            }
            _dbContext.Groups.Remove(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GroupDTO>> GetGroups(Grade? grade)
        {
            return await _dbContext.Groups.Where(x=> grade==null? true : x.Grade==grade).Select(x=>new GroupDTO(x)).ToListAsync();
        }
    }
}
