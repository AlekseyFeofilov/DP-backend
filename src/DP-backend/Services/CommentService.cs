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
        Task AddComment(AddCommentDTO addComment, Guid authorId);
        Task<List<CommentDTO>> GetComments(Guid entityId);
        Task DeleteComment(Guid commentId);
    }

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddComment(AddCommentDTO addComment, Guid authorId)
        {
            var authorUser = _context.Users.FirstOrDefault(u => u.Id == authorId);
            if (authorUser == null)
            {
                throw new NotFoundException($"Пользователь {authorId} не найден");
            }

            var newComment = new Comment
            {
                CreatedBy = authorUser.Id,
                TargetEntityId = addComment.EntityId,
                Message = addComment.Message,
            };
            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentDTO>> GetComments(Guid entityId)
        {
            var comments = await _context.Comments.GetUndeleted()
                .Where(c => c.TargetEntityId == entityId)
                .Join(_context.Users, c => c.CreatedBy, u => u.Id, (c, u) => new { Comment = c, User = u })
                .OrderBy(c => c.Comment.CreateDateTime)
                .Select(c => new CommentDTO(c.Comment, c.User))
                .ToListAsync();

            return comments;
        }

        public async Task DeleteComment(Guid commentId)
        {
            var comment = await _context.Comments.GetUndeleted().FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                throw new NotFoundException($"Комментарий {commentId} не найден");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
