using Azure.Core;
using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Migrations;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Reactive;

namespace DP_backend.Services
{
    public interface IInternshipDiaryRequestService
    {
        Task Create(InternshipDiaryRequestCreationDTO creationDTO);
        Task<List<InternshipDiaryRequestDTO>> GetAllByStatus(InternshipDiaryRequestStatus? status);
        Task<InternshipDiaryRequestDTO> GetById(Guid id);
        Task<List<InternshipDiaryRequestDTO>> GetByStudentId(Guid studentId);
        Task ChangeStatus(Guid requestId, InternshipDiaryRequestStatus newStatus, Guid userId, bool isStudent);
        Task Delete(Guid id);
        Task SetGrade(Guid id, float mark);
    }

    public class InternshipDiaryRequestService : IInternshipDiaryRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        private readonly string _staffNotification = "http://dp-staff.alexfil888.fvds.ru/internship-diary/";
        private readonly string _studentNotification = "http://dp-student.alexfil888.fvds.ru/internship-diary#";

        public InternshipDiaryRequestService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        
        public async Task Create(InternshipDiaryRequestCreationDTO creationDTO)
        {
            if (creationDTO.Semester < 5 || creationDTO.Semester > 8)
            {
                throw new BadDataException("Некорректное значение семестра. Допустимые значения 5,6,7,8");
            }
            var student = await _context.Students.GetUndeleted()
                .FirstOrDefaultAsync(s => s.Id == creationDTO.StudentId);
            if (student == null)
            {
                throw new BadDataException($"Студент {creationDTO.StudentId} не найден");
            }
            var existingRequest = await _context.InternshipDiaryRequests.GetUndeleted()
                .FirstOrDefaultAsync(r => r.StudentId == creationDTO.StudentId && r.Semester == creationDTO.Semester);
            if (existingRequest != null)
            {
                throw new BadDataException($"Заявка на {creationDTO.Semester} уже существует у студента {creationDTO.StudentId}");
            }

            var newRequest = new InternshipDiaryRequest()
            {
                StudentId = (Guid)creationDTO.StudentId,
                Semester = creationDTO.Semester,
                Status = InternshipDiaryRequestStatus.No,
                InternshipRequestId = creationDTO.InternshipRequestId
            };

            await _context.InternshipDiaryRequests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InternshipDiaryRequestDTO>> GetAllByStatus(InternshipDiaryRequestStatus? status)
        {
            var requests = await _context.InternshipDiaryRequests.GetUndeleted()
                .If(status != null, q => q.Where(r => r.Status == status))
                .Include(f => f.Student)
                .GroupJoin(_context.FileEntityLinks.Include(f => f.File), r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, Files = f })
                .Select(r => new InternshipDiaryRequestDTO(r.Request, r.Files))
                .ToListAsync();
            return requests;
        }

        public async Task<InternshipDiaryRequestDTO> GetById(Guid id)
        {
            var request = await _context.InternshipDiaryRequests.GetUndeleted()
                .Where(r => r.Id == id)
                .Include(f => f.Student)
                .GroupJoin(_context.FileEntityLinks.Include(f => f.File), r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, Files = f })
                .Select(r => new InternshipDiaryRequestDTO(r.Request, r.Files))
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
               .Include(f => f.Student)
               .GroupJoin(_context.FileEntityLinks.Include(f => f.File), r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, Files = f })
               .Select(r => new InternshipDiaryRequestDTO(r.Request, r.Files))
               .ToListAsync();
            return requests;
        }

        public async Task ChangeStatus(Guid requestId, InternshipDiaryRequestStatus newStatus, Guid userId, bool isStudent)
        {
            var request = await _context.InternshipDiaryRequests.GetUndeleted()
                .Where(r => r.Id == requestId)
                .Include(s => s.Student)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на дневник практики {requestId} не найдена");
            }

            if (isStudent)
            {
                if ((request.Status == InternshipDiaryRequestStatus.No && newStatus == InternshipDiaryRequestStatus.OnVerification) || 
                    (request.Status == InternshipDiaryRequestStatus.OnRevision && newStatus == InternshipDiaryRequestStatus.OnVerification ||
                    request.Status == InternshipDiaryRequestStatus.Approved && newStatus == InternshipDiaryRequestStatus.SubmittedForSigning))
                {
                    if (request.StudentId == userId)
                    {
                        request.Status = newStatus;
                        if (newStatus == InternshipDiaryRequestStatus.OnVerification)
                        {
                            var notification = new NotificationCreationDTO
                            {
                                Title = $"Отправлен на проверку дневник практики за {request.Semester} семестр",
                                Message = $"Студент {request.Student.Name} группы {request.Student.Group?.Number} отправил на проверку дневник практики за {request.Semester} семестр",
                                Link = _staffNotification + request.Id
                            };
                            await _notificationService.CreateNotificationForStaff(notification);
                        }
                        if (newStatus == InternshipDiaryRequestStatus.SubmittedForSigning)
                        {
                            var notification = new NotificationCreationDTO
                            {
                                Title = $"Сдан на подпись дневник практики за {request.Semester} семестр",
                                Message = $"Студент {request.Student.Name} группы {request.Student.Group?.Number} сдал на подпись дневник практики за {request.Semester} семестр",
                                Link = _staffNotification + request.Id
                            };
                            await _notificationService.CreateNotificationForStaff(notification);
                        }
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
                var notification = new NotificationCreationDTO
                {
                    Title = $"Изменён статус дневника практики за {request.Semester} семестр",
                    Message = $"Статус дневника практики за {request.Semester} семестр изменён на {newStatus}",
                    Link = _studentNotification + request.Id,
                    AddresseeId = request.Student.UserId
                };
                await _notificationService.Create(notification);
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

        public async Task SetGrade(Guid id, float mark)
        {
            var request = await _context.InternshipDiaryRequests
                .Where(r => r.Id == id)
                .Include(s => s.Student)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на дневник практики {id} не найдена");
            }
            request.Mark = mark;
            request.Status = InternshipDiaryRequestStatus.Rated;
            await _context.SaveChangesAsync();

            var notification = new NotificationCreationDTO
            {
                Title = $"Дневник практики за {request.Semester} семестр оценён",
                Message = $"Поставлена оценка \'{mark}\' дневнику практики за {request.Semester} семестр",
                Link = _studentNotification + request.Id,
                AddresseeId = request.Student.UserId
            };
            await _notificationService.Create(notification);
        }
    }
}
