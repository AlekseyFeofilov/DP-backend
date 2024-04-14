using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IEmploymentService
    {
        Task<List<EmploymentDTO>> GetEmployments(string? companyName, EmploymentStatus? employmentStatus);
        Task<EmploymentDTO> GetEmployment(Guid employmentId, Guid userId, bool isStaff);
        Task CreateEmployment(Guid employmentId, EmploymentСreationDTO employmentСreation);
        Task ChangeEmployment(Guid employmentId, EmploymentChangeDTO employmentChange, Guid userId, bool isStaff);
        Task DeleteEmployment(Guid employmentId);
    }

    public class EmploymentService : IEmploymentService
    {
        private readonly ApplicationDbContext _context;

        public EmploymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmploymentDTO>> GetEmployments(string? companyName, EmploymentStatus? employmentStatus)
        {
            IQueryable<Employment> employmentsQuery = _context.Employments.GetUndeleted().Include(e => e.Employer);
            if (companyName != null)
            {
                employmentsQuery = employmentsQuery.Where(e => e.Employer.CompanyName.Contains(companyName));
            }
            if (employmentStatus != null)
            {
                employmentsQuery = employmentsQuery.Where(e => e.Status == employmentStatus);
            }

            return await employmentsQuery
                .Select(employment => new EmploymentDTO(employment))
                .ToListAsync();
        }

        public async Task<EmploymentDTO> GetEmployment(Guid employmentId, Guid userId, bool isStaff)
        {
            var employment = await _context.Employments.GetUndeleted()
                .Include(e => e.Employer)
                .FirstOrDefaultAsync(e => e.Id == employmentId);
            if (employment == null)
            {
                throw new NotFoundException($"Трудоустройство {employmentId} не найдено");
            }
            if (employment.StudentId != userId && !isStaff)
            {
                throw new NoPermissionException();
            }
            return new EmploymentDTO(employment);
        }

        public async Task CreateEmployment(Guid userId, EmploymentСreationDTO employmentСreation)
        {
            var existingEmployment = await _context.Employments.GetUndeleted().FirstOrDefaultAsync(e => e.StudentId == userId);
            if (existingEmployment != null)
            {
                throw new BadDataException($"У студента {userId} уже есть трудоустройство");
            }
            var employer = await _context.Employers.GetUndeleted().FirstOrDefaultAsync(e => e.Id == employmentСreation.EmployerId);
            if (employer == null)
            {
                throw new BadDataException($"Компания-работодатель {employmentСreation.EmployerId} не найдена");
            }
            var employment = new Employment
            {
                StudentId = userId,
                Employer = employer,
                Vacancy = employmentСreation.Vacancy,
                Comment = employmentСreation.Comment,
                Status = EmploymentStatus.NonVerified,
            };
            await _context.Employments.AddAsync(employment);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeEmployment(Guid employmentId, EmploymentChangeDTO employmentChange, Guid userId, bool isStaff)
        {
            var employment = await _context.Employments.GetUndeleted().FirstOrDefaultAsync(e => e.Id == employmentId);
            if (employment == null)
            {
                throw new NotFoundException($"Трудоустройство {employmentId} не найдено");
            }
            var newEmployer = await _context.Employers.GetUndeleted().FirstOrDefaultAsync(e => e.Id == employmentChange.EmployerId);
            if (newEmployer == null)
            {
                throw new BadDataException($"Компания-работодатель {employmentChange.EmployerId} не найдена");
            }
            employment.Employer = newEmployer;
            employment.Vacancy = employmentChange.Vacancy;
            employment.Comment = employmentChange.Comment;
            if (employment.Status != employmentChange.EmploymentStatus && employmentChange.EmploymentStatus == EmploymentStatus.Verified && isStaff)
            {
                employment.Status = employmentChange.EmploymentStatus;
            }
            else
            {
                throw new NoPermissionException("Подтвердить трудоустройство может только сотрудник");
            }
            if (!isStaff)
            {
                employment.Status = EmploymentStatus.NonVerified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployment(Guid employmentId)
        {
            var employment = await _context.Employments.GetUndeleted().FirstOrDefaultAsync(e => e.Id == employmentId);
            if (employment == null)
            {
                throw new NotFoundException($"Трудоустройство {employmentId} не найдено");
            }
            _context.Employments.Remove(employment);
            await _context.SaveChangesAsync();
        }
    }
}
