using DP_backend.Common;
using DP_backend.Common.EntityType;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.Services
{
    public interface ICommentService
    {
        Task AddComment(AddCommentDTO addComment, Guid authorId, CancellationToken ct);
        Task<List<CommentDTO>> GetComments(string entityType, string entityId, CancellationToken ct);
        Task DeleteComment(Guid commentId, CancellationToken ct);
    }

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEntityTypesProvider _entityTypesProvider;
        private readonly INotificationService _notificationService;

        private readonly string _staffDiaryNotification = "http://dp-staff.alexfil888.fvds.ru/internship-diary/";
        private readonly string _studentDiaryNotification = "http://dp-student.alexfil888.fvds.ru/internship-diary#";
        private readonly string _staffIntershipNotification = "http://dp-staff.alexfil888.fvds.ru/statement/internship-check#";
        private readonly string _studentIntershipNotification = "http://dp-student.alexfil888.fvds.ru/statement/internship-check#";
        private readonly string _staffEmploymentNotification = "http://dp-staff.alexfil888.fvds.ru/statement/internship-apply#";
        private readonly string _studentEmploymentNotification = "http://dp-student.alexfil888.fvds.ru/statement/internship-apply#";
        //private readonly string _staffEmploymentVariantNotification = "http://dp-staff.alexfil888.fvds.ru/statement/internship-apply#";
        private readonly string _studentEmploymentVariantNotification = "http://dp-student.alexfil888.fvds.ru/employment-variant#";

        public CommentService(ApplicationDbContext context, IEntityTypesProvider entityTypesProvider, INotificationService notificationService)
        {
            _context = context;
            _entityTypesProvider = entityTypesProvider;
            _notificationService = notificationService;
        }

        public async Task AddComment(AddCommentDTO addComment, Guid authorId, CancellationToken ct)
        {
            var authorUser = _context.Users.FirstOrDefault(u => u.Id == authorId);
            if (authorUser == null)
            {
                throw new NotFoundException($"Пользователь {authorId} не найден");
            }

            if (await _entityTypesProvider.ValidateEntityType(addComment.EntityType, EntityTypeUsage.LinkComment, ct) == false)
            {
                throw new BadDataException($"Invalid entity type \"{addComment.EntityType}\"");
            }

            var newComment = new Comment
            {
                CreatedBy = authorUser.Id,
                TargetEntityType = addComment.EntityType,
                TargetEntityId = addComment.EntityId,
                Message = addComment.Message,
            };

            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();

            await SendNotification(addComment, authorUser);
        }

        public async Task<List<CommentDTO>> GetComments(string entityType, string entityId, CancellationToken ct)
        {
            var comments = await _context.Comments.GetUndeleted()
                .Where(c => c.TargetEntityType == entityType && c.TargetEntityId == entityId)
                .Join(_context.Users, c => c.CreatedBy, u => u.Id, (c, u) => new { Comment = c, User = u })
                .OrderBy(c => c.Comment.CreateDateTime)
                .Select(c => new CommentDTO(c.Comment, c.User))
                .ToListAsync(ct);

            return comments;
        }

        public async Task DeleteComment(Guid commentId, CancellationToken ct)
        {
            var comment = await _context.Comments.GetUndeleted().FirstOrDefaultAsync(c => c.Id == commentId, ct);
            if (comment == null)
            {
                throw new NotFoundException($"Комментарий {commentId} не найден");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(ct);
        }

        private async Task SendNotification(AddCommentDTO addComment, User author)
        {
            string titleEnding;
            string link;
            bool forStaff = false;
            switch (addComment.EntityType)
            {
                case "InternshipRequest":
                    var internshipRequests = await _context.InternshipRequests.GetUndeleted().FirstOrDefaultAsync(r => r.Id.ToString() == addComment.EntityId);
                    if (internshipRequests == null)
                    {
                        return;
                    }
                    if (author.Id == internshipRequests.StudentId)
                    {
                        link = _staffIntershipNotification;
                        forStaff = true;
                    }
                    else
                    {
                        link = _studentIntershipNotification;
                    }
                    titleEnding = " к заявке на прохождение практики";
                    break;
                case "EmploymentRequest":
                    var employmentRequests = await _context.EmploymentRequests.GetUndeleted().FirstOrDefaultAsync(r => r.Id.ToString() == addComment.EntityId);
                    if (employmentRequests == null)
                    {
                        return;
                    }
                    if (author.Id == employmentRequests.StudentId)
                    {
                        link = _staffEmploymentNotification;
                        forStaff = true;
                    }
                    else
                    {
                        link = _studentEmploymentNotification;
                    }
                    titleEnding = " к заявке на заведения трудоустройства";
                    break;
                case "EmploymentVariant":
                    var employmentVariants = await _context.EmploymentVariants.GetUndeleted().FirstOrDefaultAsync(r => r.Id.ToString() == addComment.EntityId);
                    if (employmentVariants == null)
                    {
                        return;
                    }
                    if (author.Id == employmentVariants.StudentId)
                    {
                        return;
                    }
                    else
                    {
                        link = _studentEmploymentVariantNotification;
                    }
                    titleEnding = " к варианту трудоустройства";
                    break;
                case "InternshipDiaryRequest":
                    var internshipDiaryRequest = await _context.InternshipDiaryRequests.GetUndeleted().FirstOrDefaultAsync(r => r.Id.ToString() == addComment.EntityId);
                    if (internshipDiaryRequest == null)
                    {
                        return;
                    }
                    if (author.Id == internshipDiaryRequest.StudentId)
                    {
                        link = _staffDiaryNotification;
                        forStaff = true;
                    }
                    else
                    {
                        link = _studentDiaryNotification;
                    }
                    titleEnding = " к заявке на дневник практики";
                    break;
                case "CourseWorkRequest":
                    return;
                default:
                    return;
            }
            string comment;
            if (addComment.Message.Length > 40)
            {
                comment = (string)addComment.Message.Take(40) + "...";
            }
            else
            {
                comment = addComment.Message;
            }
            var notification = new NotificationCreationDTO
            {
                Title = "Новый комментарий" + titleEnding,
                Message = $"{author.UserName} добавил(-а) комментарий: \'{comment}\'",
                Link = link + addComment.EntityId
            };
            if (forStaff)
            {
                await _notificationService.CreateNotificationForStaff(notification);
            }
            else
            {
                notification.AddresseeId = author.Id;
                await _notificationService.Create(notification);
            }
        }
    }
}