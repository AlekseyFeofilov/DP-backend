using DP_backend.Common.Enumerations;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Migrations;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IEmploymentService
    {
        Task<List<EmploymentDTO>> GetEmployments(string? companyName, EmploymentStatus? employmentStatus);
        Task<EmploymentDTO> GetEmployment(Guid employmentId, Guid userId);
        Task CreateEmploymentRequest(Guid internshipRequestId, Guid userId);
        Task UpdateEmploymentRequestStatus(Guid employmentRequestId, EmploymentRequestStatus status);
        Task<InternshipRequest> CreateInternshipRequest(Guid userId, InternshipRequestСreationDTO employmentСreation);
        Task CreateEmployment(EmploymentRequest internshipRequest);
        Task UpdateInternshipRequestStatus(Guid internshipRequestId, InternshipStatus status);
        Task<List<InternshipRequestDTO>> GetStudentInternshipRequests(Guid studentId);
        Task<List<InternshipRequestDTO>> GetNonVerifiedInternshipRequests();
        Task ChangeEmployment(Guid employmentId, EmploymentChangeDTO employmentChange, Guid userId, bool isStaff);
        Task DeleteEmployment(Guid employmentId);
        Task<List<EmploymentRequestDTO>> GetStudentRequests(Guid studentId);
        Task<List<EmploymentDTO>> GetStudentEmployments(Guid studentId);
        Task<List<ObjectWithDataDTO<EmploymentsInfoDTO>>> GetAllStudentEmploymentInformation(Guid studentId);
        Task<InternshipRequestDTO> GetInternshipRequest(Guid internshipRequestId);
        Task<List<InternshipRequestDTO>> GetInternshipRequestsWithFilters(int? group, InternshipStatus? status);
        Task<EmploymentRequestDTO> GetEmploymentRequest(Guid employmentRequestId);
        Task<List<EmploymentRequestDTO>> GetEmploymentRequestsWithFilters(int? group, EmploymentRequestStatus? status);
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

        public async Task<EmploymentDTO> GetEmployment(Guid employmentId, Guid userId)
        {
            var employment = await _context.Employments.GetUndeleted()
                .Include(e => e.Employer)
                .FirstOrDefaultAsync(e => e.Id == employmentId);
            if (employment == null)
            {
                throw new NotFoundException($"Трудоустройство {employmentId} не найдено");
            }
            return new EmploymentDTO(employment);
        }

        public async Task<InternshipRequest> CreateInternshipRequest(Guid userId, InternshipRequestСreationDTO employmentСreation)
        {

            var employer = await _context.Employers.GetUndeleted().FirstOrDefaultAsync(e => e.Id == employmentСreation.EmployerId);
            if (employer == null)
            {
                throw new BadDataException($"Компания-работодатель {employmentСreation.EmployerId} не найдена");
            }
            var internshipRequest = new InternshipRequest
            {
                StudentId = userId,
                Employer = employer,
                Vacancy = employmentСreation.Vacancy,
                Comment = employmentСreation.Comment,
                Status = InternshipStatus.NonVerified,
                Id= Guid.NewGuid()
            };
            await _context.InternshipRequests.AddAsync(internshipRequest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            return internshipRequest;
        }

        public async Task ChangeEmployment(Guid employmentId, EmploymentChangeDTO employmentChange, Guid userId, bool isStaff)
        {
            var employment = await _context.Employments.GetUndeleted().FirstOrDefaultAsync(e => e.Id == employmentId);
            if (employment == null)
            {
                throw new NotFoundException($"Трудоустройство {employmentId} не найдено");
            }
            employment.Vacancy = employmentChange.Vacancy;
            employment.Comment = employmentChange.Comment;
            if (isStaff)
            {
                employment.Status = employmentChange.EmploymentStatus;
            }
            else
            {
                throw new NoPermissionException("Подтвердить трудоустройство может только сотрудник");
            }
            if (!isStaff)
            {
                employment.Status = EmploymentStatus.InActive;
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

        public async Task UpdateInternshipRequestStatus(Guid internshipRequestId, InternshipStatus status)
        {
            var internshipRequest = await _context.InternshipRequests.Include(x => x.Employer).FirstOrDefaultAsync(e => e.Id == internshipRequestId);
            if (internshipRequest == null)
            {
                throw new NotFoundException($"Заявка на прохождение практики в компании {internshipRequestId} не найдено");
            }
            internshipRequest.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task<List<InternshipRequestDTO>> GetStudentInternshipRequests(Guid studentId)
        {
            var internshipRequests = await _context.InternshipRequests.Include(x => x.Employer).Include(x=>x.Student).ThenInclude(x=>x.Group).Where(e => e.StudentId == studentId).Select(x=> new InternshipRequestDTO(x)).ToListAsync();
            return internshipRequests;
        }

        public async Task<List<InternshipRequestDTO>> GetNonVerifiedInternshipRequests()
        {
            var internshipRequests = await _context.InternshipRequests.Include(x=>x.Employer).Include(x => x.Student).ThenInclude(x => x.Group).Where(e => e.Status == InternshipStatus.NonVerified).Select(x => new InternshipRequestDTO(x)).ToListAsync();
            return internshipRequests;
        }

        public async Task CreateEmploymentRequest(Guid internshipRequestId, Guid userId)
        {
            var internshipRequest = await _context.InternshipRequests.Include(x=>x.Employer).FirstOrDefaultAsync(x => x.Id == internshipRequestId && x.StudentId == userId);
            if (internshipRequest == null)
            {
                throw new NotFoundException($"Заявка на прохождение практики в компании {internshipRequestId} у вас не найдено");
            }
            if (internshipRequest.Status != InternshipStatus.Accepted)
            {
                throw new InvalidOperationException($"Нельзя создать заявку на создание трудоустройства на основе заявка на прохождение практики в компании со статусом {internshipRequest.Status}");
            }
            var employmentRequest = new EmploymentRequest
            {
                Id = new Guid(),
                StudentId = userId,
                InternshipRequestId = internshipRequestId,
                Status = EmploymentRequestStatus.NonVerified,              
            };
            await _context.EmploymentRequests.AddAsync(employmentRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmploymentRequestStatus(Guid employmentRequestId, EmploymentRequestStatus status)
        {
            var employmentRequest = await _context.EmploymentRequests
                .Include(x=>x.InternshipRequest)
                .ThenInclude(x=>x.Employer)
                .FirstOrDefaultAsync(x => x.Id == employmentRequestId);
            if (employmentRequest == null)
            {
                throw new NotFoundException($"Заявка на заведение трудоустройства в компании {employmentRequestId} у вас не найдено");
            }
            employmentRequest.Status = status;
            if (status == EmploymentRequestStatus.Accepted)
            {
              await CreateEmployment(employmentRequest);
            }            
            await _context.SaveChangesAsync();           
        }

        public async Task CreateEmployment(EmploymentRequest employmentRequest)
        {
            var student = await _context.Students.Include(x=>x.InternshipRequests).Include(x => x.EmploymentRequests).Include(x => x.Employments).FirstOrDefaultAsync(x => x.Id == employmentRequest.InternshipRequest.StudentId);
            if(student == null)
            {
                throw new NotFoundException($"Пользователь с Id {employmentRequest.StudentId}  не найден");
            }
            if (student.Employments.Select(x=>x.Id).Contains(employmentRequest.Id))
            {
                throw new InvalidOperationException($"Уже существует трудоустройство для заявки на создание трудоустройства с Id {employmentRequest.StudentId} уже существует!");
            }
            if (employmentRequest.Status != EmploymentRequestStatus.Accepted)
            {
                throw new BadDataException($"Нельзя создать трудоустройство на основе заявки на создание трудоустройства со статусом {employmentRequest.Status}");
            }
            var employment = new Employment
            {
                Id = employmentRequest.Id,
                Comment = employmentRequest.InternshipRequest.Comment,
                Status = EmploymentStatus.Active,
                Employer = employmentRequest.InternshipRequest.Employer,
                StudentId = employmentRequest.InternshipRequest.StudentId,
                Vacancy = employmentRequest.InternshipRequest.Vacancy,
                EmploymentRequestId= employmentRequest.Id,
                InternshipRequestId = employmentRequest.InternshipRequestId,
            };
            student.Employments.ForEach(x => x.Status = EmploymentStatus.InActive);
            student.EmploymentRequests.ForEach(x => { 
                    x.Status = EmploymentRequestStatus.UnActual;
            });
            student.InternshipRequests.ForEach(x =>
            {
                if (x.Status == InternshipStatus.Accepted || x.Status== InternshipStatus.NonVerified)
                {
                    x.Status = InternshipStatus.Unactual;
                }              
            });
            await _context.Employments.AddAsync(employment);
            await _context.SaveChangesAsync();

        }

        public async Task<List<EmploymentRequestDTO>> GetStudentRequests(Guid studentId)
        {
            var student = await _context.Students
                .Include(x => x.EmploymentRequests)
                .ThenInclude(x=>x.InternshipRequest)
                .ThenInclude(x=>x.Employer)
                .Include(x => x.EmploymentRequests)
                .Include(x=>x.Group)
                .FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new NotFoundException($"Пользователь с Id {studentId}  не найден");
            }
            var requests = student.EmploymentRequests.Select(x => new EmploymentRequestDTO(x)).ToList();
            return requests;
        }

        public async Task<List<EmploymentDTO>> GetStudentEmployments(Guid studentId)
        {
            var student = await _context.Students
                .Include(x => x.Employments)
                .ThenInclude(x => x.Employer)
                .FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new NotFoundException($"Пользователь с Id {studentId}  не найден");
            }
            var employments = student.Employments.Select(x => new EmploymentDTO(x)).ToList();
            return employments;
        }

        public async Task<List<ObjectWithDataDTO<EmploymentsInfoDTO>>> GetAllStudentEmploymentInformation(Guid studentId)
        {
            var student = await _context.Students
                .Include(x => x.EmploymentRequests)
                .ThenInclude(x => x.InternshipRequest)
                .ThenInclude(x => x.Employer)
                .Include(x => x.Employments)
                .ThenInclude(x => x.Employer)
                .Include(x=>x.InternshipRequests)
                .ThenInclude(x => x.Employer)
                .FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null)
            {
                throw new NotFoundException($"Пользователь с Id {studentId}  не найден");
            }
            var infoList = new List<ObjectWithDataDTO<EmploymentsInfoDTO>>();
            foreach (var internshipRequest in student.InternshipRequests)
            {
                var employmentRequest = student.EmploymentRequests.Where(x=>x.InternshipRequestId == internshipRequest.Id).FirstOrDefault();
                var employment = employmentRequest==null? null : student.Employments.Where(x => x.InternshipRequestId == internshipRequest.Id && x.EmploymentRequestId==employmentRequest.Id).FirstOrDefault();
                var employmentsInfo = new EmploymentsInfoDTO 
                { 
                    Employment= employment == null ? null : new EmploymentDTO(employment),
                    EmploymentRequest = employmentRequest==null? null: new EmploymentRequestDTO(employmentRequest),
                    InternshipRequest =  new InternshipRequestDTO(internshipRequest)
                };
                var info = new ObjectWithDataDTO<EmploymentsInfoDTO>
                {
                    Date = employment!=null? employment.ModifyDateTime : employmentRequest != null? employmentRequest.ModifyDateTime: internshipRequest.ModifyDateTime,
                    Object= employmentsInfo
                };
                infoList.Add(info);
            }
            return infoList;
        }

        public async Task<InternshipRequestDTO> GetInternshipRequest(Guid internshipRequestId)
        {
            var internshipRequest = await _context.InternshipRequests.Include(x=>x.Employer).Include(x => x.Student).ThenInclude(x=>x.Group).FirstOrDefaultAsync(x=>x.Id==internshipRequestId);
            if(internshipRequest==null)
            {
                throw new NotFoundException($"Заявка 1ого типа с Id {internshipRequestId}  не найдена");
            }
            return new InternshipRequestDTO(internshipRequest);
        }

        public async Task<List<InternshipRequestDTO>> GetInternshipRequestsWithFilters(int? group, InternshipStatus? status)
        {
            IQueryable<InternshipRequest> query = _context.InternshipRequests.Include(x => x.Student).ThenInclude(x => x.Group).Include(x => x.Employer);
            if (group != null)
            {
                query = query.Where(x => x.Student.Group.Number == group);
            }
            if(status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            return await query.Select(x=>new InternshipRequestDTO(x)).ToListAsync();
        }

        public async Task<EmploymentRequestDTO> GetEmploymentRequest(Guid employmentRequestId)
        {
            var employmentRequest = await _context.EmploymentRequests
                .Include(x => x.Student)
                .ThenInclude(x => x.Group)
                .Include(x => x.InternshipRequest)
                .ThenInclude(x => x.Employer)
                .Include(x => x.InternshipRequest)
                .ThenInclude(x => x.Student)
                .ThenInclude(x => x.Group)
                .FirstOrDefaultAsync(x => x.Id == employmentRequestId);
            if (employmentRequest == null)
            {
                throw new NotFoundException($"Заявка 2ого типа с Id {employmentRequestId}  не найдена");
            }
            return new EmploymentRequestDTO(employmentRequest);
        }

        public async Task<List<EmploymentRequestDTO>> GetEmploymentRequestsWithFilters(int? group, EmploymentRequestStatus? status)
        {
            IQueryable<EmploymentRequest> query = _context.EmploymentRequests
                .Include(x=>x.InternshipRequest)
                .ThenInclude(x => x.Student)
                .ThenInclude(x => x.Group)
                .Include(x => x.InternshipRequest)
                .ThenInclude(x => x.Employer);
            if (group != null)
            {
                query = query.Where(x => x.InternshipRequest.Student.Group.Number == group);
            }
            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }
            return await query.Select(x => new EmploymentRequestDTO(x)).ToListAsync();
        }
    }
}
