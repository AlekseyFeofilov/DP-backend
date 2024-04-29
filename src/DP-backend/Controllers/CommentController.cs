using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> AddComment(AddCommentDTO addComment)
        {
            await _commentService.AddComment(addComment, User.GetUserId());
            return Ok();
        }

        [HttpGet]
        [Route("{entityId:guid}")]
        public async Task<ActionResult<List<CommentDTO>>> GetComment(Guid entityId)
        {
            var comments = await _commentService.GetComments(entityId);
            return Ok(comments);
        }

        [HttpDelete]
        [Route("{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            await _commentService.DeleteComment(commentId);
            return Ok();
        }
    }
}
