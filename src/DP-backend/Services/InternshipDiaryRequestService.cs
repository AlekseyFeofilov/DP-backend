using Azure.Core;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface IInternshipDiaryRequestService
    {
        Task Create(InternshipDiaryRequestCreationDTO creationDTO);
        Task<List<InternshipDiaryRequestDTO>> GetAllByStatus(InternshipDiaryRequestStatus status);
        Task<InternshipDiaryRequestDTO> GetById(Guid id);
        Task<List<InternshipDiaryRequestDTO>> GetByStudentId(Guid studentId);
        Task ChangeStatus(Guid requestId, InternshipDiaryRequestStatus newStatus, Guid userId, bool isStudent);
        Task Delete(Guid id);
    }

    public class InternshipDiaryRequestService : IInternshipDiaryRequestService
    {
        private readonly ApplicationDbContext _context;

        public InternshipDiaryRequestService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task Create(InternshipDiaryRequestCreationDTO creationDTO)
        {
            if (creationDTO.Semester < 5 || creationDTO.Semester > 8)
            {
                throw new BadDataException("Некорректное значение семестра. Допустимые значения 5,6,7,8");
            }
            var existingRequest = await _context.InternshipDiaryRequests.GetUndeleted()
                .FirstOrDefaultAsync(r => r.StudentId == creationDTO.StudentId && r.Semester == creationDTO.Semester);
            if (existingRequest != null)
            {
                throw new BadDataException($"Заявка на {creationDTO.Semester} уже существует у студента {creationDTO.StudentId}");
            }

            var newRequest = new InternshipDiaryRequest()
            {
                StudentId = creationDTO.StudentId,
                Semester = creationDTO.Semester,
                Status = InternshipDiaryRequestStatus.No,
            };
            await _context.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InternshipDiaryRequestDTO>> GetAllByStatus(InternshipDiaryRequestStatus status)
        {
            var requests = await _context.InternshipDiaryRequests.GetUndeleted()
                .Where(r => r.Status == status)
                .GroupJoin(_context.FileEntityLinks, r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, FileIds = f.Select(f => f.FileId).ToList() })
                .Select(r => new InternshipDiaryRequestDTO(r.Request, r.FileIds))
                .ToListAsync();
            return requests;
        }

        public async Task<InternshipDiaryRequestDTO> GetById(Guid id)
        {
            var request = await _context.InternshipDiaryRequests.GetUndeleted()
                .Where(r => r.Id == id)
                .GroupJoin(_context.FileEntityLinks, r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, FileIds = f.Select(f => f.FileId).ToList() })
                .Select(r => new InternshipDiaryRequestDTO(r.Request, r.FileIds))
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на дневник практики {id} не найдена");
            }
            return request;
        }

        public async Task<List<InternshipDiaryRequestDTO>> GetByStudentId(Guid studentId)
        {
            var requests = await _context.InternshipDiaryRequests.GetUndeleted()
               .Where(r => r.StudentId == studentId)
                .GroupJoin(_context.FileEntityLinks, r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, FileIds = f.Select(f => f.FileId).ToList() })
                .Select(r => new InternshipDiaryRequestDTO(r.Request, r.FileIds))
               .ToListAsync();
            return requests;
        }

        public async Task ChangeStatus(Guid requestId, InternshipDiaryRequestStatus newStatus, Guid userId, bool isStudent)
        {
            var request = await _context.InternshipDiaryRequests.GetUndeleted()
                .Where(r => r.Id == requestId)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на дневник практики {requestId} не найдена");
            }

            if (isStudent)
            {
                if ((request.Status == InternshipDiaryRequestStatus.No && newStatus == InternshipDiaryRequestStatus.OnVerification) || 
                    (request.Status == InternshipDiaryRequestStatus.OnRevision && newStatus == InternshipDiaryRequestStatus.OnVerification))
                {
                    if (request.StudentId == userId)
                    {
                        request.Status = newStatus;
                    }
                    else
                    {
                        throw new NoPermissionException($"Нельзя поменять статус чужой заявки");
                    }
                }
                else
                {
                    throw new NoPermissionException($"Студент не может поменять статус с {request.Status} на {newStatus}");
                }
            }
            else
            {
                request.Status = newStatus;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var request = await _context.InternshipDiaryRequests.GetUndeleted()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на дневник практики {id} не найдена");
            }
            _context.InternshipDiaryRequests.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}
