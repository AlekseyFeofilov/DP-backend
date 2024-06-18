using Azure.Core;
using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface ICourseWorkRequestService
    {
        Task Create(CourseWorkRequestCreationDTO creationDTO);
        Task<List<CourseWorkRequestDTO>> GetAllByStatus(CourseWorkRequestStatus? status);
        Task<CourseWorkRequestDTO> GetById(Guid id);
        Task<List<CourseWorkRequestDTO>> GetByStudentId(Guid studentId);
        Task ChangeStatus(Guid requestId, CourseWorkRequestStatus newStatus, Guid userId, bool isStudent);
        Task Delete(Guid id);
        Task SetGrade (Guid id, float mark);
    }

    public class CourseWorkRequestService : ICourseWorkRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        //private readonly string _staffNotification = "http://dp-staff.alexfil888.fvds.ru/internship-diary/";
        //private readonly string _studentNotification = "http://dp-student.alexfil888.fvds.ru/internship-diary#";

        public CourseWorkRequestService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task Create(CourseWorkRequestCreationDTO creationDTO)
        {
            if (creationDTO.Semester != 6 && creationDTO.Semester != 8)
            {
                throw new BadDataException("Некорректное значение семестра. Допустимые значения 6,8");
            }
            var student = await _context.Students.GetUndeleted()
                .FirstOrDefaultAsync(s => s.Id == creationDTO.StudentId);
            if (student == null)
            {
                throw new BadDataException($"Студент {creationDTO.StudentId} не найден");
            }
            var existingRequest = await _context.CourseWorkRequests.GetUndeleted()
                .FirstOrDefaultAsync(r => r.StudentId == creationDTO.StudentId && r.Semester == creationDTO.Semester);
            if (existingRequest != null)
            {
                throw new BadDataException($"Заявка на {creationDTO.Semester} уже существует у студента {creationDTO.StudentId}");
            }

            var newRequest = new CourseWorkRequest()
            {
                StudentId = (Guid)creationDTO.StudentId,
                Semester = creationDTO.Semester,
                Status = CourseWorkRequestStatus.No,
            };
            await _context.CourseWorkRequests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseWorkRequestDTO>> GetAllByStatus(CourseWorkRequestStatus? status)
        {
            var requests = await _context.CourseWorkRequests.GetUndeleted()
               .If(status != null, q => q.Where(r => r.Status == status))
               .Include(f => f.Student)
               .GroupJoin(_context.FileEntityLinks.Include(f => f.File), r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, Files = f })
               .Select(r => new CourseWorkRequestDTO(r.Request, r.Files))
               .ToListAsync();
            return requests;
        }

        public async Task<CourseWorkRequestDTO> GetById(Guid id)
        {
            var request = await _context.CourseWorkRequests.GetUndeleted()
                .Where(r => r.Id == id)
                .Include(f => f.Student)
                .GroupJoin(_context.FileEntityLinks.Include(f => f.File), r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, Files = f })
                .Select(r => new CourseWorkRequestDTO(r.Request, r.Files))
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на курсовую/диплом {id} не найдена");
            }
            return request;
        }

        public async Task<List<CourseWorkRequestDTO>> GetByStudentId(Guid studentId)
        {
            var requests = await _context.CourseWorkRequests.GetUndeleted()
               .Where(r => r.StudentId == studentId)
               .Include(f => f.Student)
               .GroupJoin(_context.FileEntityLinks.Include(f => f.File), r => r.Id.ToString(), f => f.EntityId, (r, f) => new { Request = r, Files = f })
               .Select(r => new CourseWorkRequestDTO(r.Request, r.Files))
               .ToListAsync();
            return requests;
        }

        public async Task ChangeStatus(Guid requestId, CourseWorkRequestStatus newStatus, Guid userId, bool isStudent)
        {
            var request = await _context.CourseWorkRequests.GetUndeleted()
                .Where(r => r.Id == requestId)
                .Include(s => s.Student)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на курсовую/диплом {requestId} не найдена");
            }

            if (isStudent)
            {
                if ((request.Status == CourseWorkRequestStatus.No && newStatus == CourseWorkRequestStatus.Passed) ||
                    (request.Status == CourseWorkRequestStatus.Passed && newStatus == CourseWorkRequestStatus.No))
                {
                    if (request.StudentId == userId)
                    {
                        request.Status = newStatus;
                        //if (newStatus == CourseWorkRequestStatus.Passed)
                        //{
                        //    request.Status = newStatus;
                        //    var notification = new NotificationCreationDTO
                        //    {
                        //        Title = GetTitleForPassedBySemester(request.Semester),
                        //        Message = GetMessageForPassedBySemester(request.Semester, request.Student),
                        //        Link = _staffNotification + request.Id
                        //    };
                        //    await _notificationService.CreateNotificationForStaff(notification);
                        //}
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
            var request = await _context.CourseWorkRequests.GetUndeleted()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на курсовую/диплом {id} не найдена");
            }
            _context.CourseWorkRequests.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task SetGrade(Guid id, float mark)
        {
            var request = await _context.CourseWorkRequests
                .Include(s => s.Student)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
            if (request == null)
            {
                throw new NotFoundException($"Заявка на курсовую/диплом {id} не найдена");
            }
            request.Mark = mark;
            request.Status = CourseWorkRequestStatus.Rated;
            await _context.SaveChangesAsync();

            //var notification = new NotificationCreationDTO
            //{
            //    Title = GetTitleForMarkBySemester(request.Semester),
            //    Message = GetMessageForMarkBySemester(request.Semester, mark),
            //    Link = _studentNotification + request.Id,
            //    AddresseeId = request.Student.UserId
            //};
            //await _notificationService.Create(notification);
        }

        private string GetTitleForPassedBySemester(int semester)
        {
            if (semester == 6)
            {
                return $"Отправлена на проверку курсовая работа";
            }
            else
            {
                return $"Отправлен на проверку диплом";
            }
        }

        private string GetMessageForPassedBySemester(int semester, Student student)
        {
            if (semester == 6)
            {
                return $"Студент {student.Name} группы {student.Group?.Number} отправил на проверку курсовую работу";
            }
            else
            {
                return $"Студент {student.Name} группы {student.Group?.Number} отправил на проверку диплом";
            }
        }

        private string GetTitleForMarkBySemester(int semester)
        {
            if (semester == 6)
            {
                return $"Курсовая работа оценена";
            }
            else
            {
                return $"Диплом оценён";
            }
        }

        private string GetMessageForMarkBySemester(int semester, float mark)
        {
            if (semester == 6)
            {
                return $"Поставлена оценка \'{mark}\' за курсовую работу";
            }
            else
            {
                return $"Поставлена оценка \'{mark}\' за диплом";
            }
        }
    }
}
