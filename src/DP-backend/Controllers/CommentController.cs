using DP_backend.Common.EntityType;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "StaffAndStudent")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentDTO addComment, CancellationToken ct)
        {
            await _commentService.AddComment(addComment, User.GetUserId(), ct);
            return Ok();
        }
        
        /// <param name="entityType">Id from <see cref="EntityType"/></param>
        /// <param name="entityId"></param>
        /// <param name="ct"></param>
        [HttpGet]
        [Route("{entityType}/{entityId}")]
        [ProducesResponseType(typeof(List<CommentDTO>), 200)]
        public async Task<ActionResult<List<CommentDTO>>> GetComment(string entityType, string entityId, CancellationToken ct)
        {
            var comments = await _commentService.GetComments(entityType, entityId, ct);
            return Ok(comments);
        }

        [HttpDelete]
        [Route("{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId, CancellationToken ct)
        {
            await _commentService.DeleteComment(commentId, ct);
            return Ok();
        }
    }
}
