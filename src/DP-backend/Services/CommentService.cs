using DP_backend.Common.EntityType;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.Employment;
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

        public CommentService(ApplicationDbContext context, IEntityTypesProvider entityTypesProvider)
        {
            _context = context;
            _entityTypesProvider = entityTypesProvider;
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
    }
}