using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseWorkRequestController : ControllerBase
    {
        private readonly ICourseWorkRequestService _courseWorkRequestService;

        public CourseWorkRequestController(ICourseWorkRequestService courseWorkRequestService)
        {
            _courseWorkRequestService = courseWorkRequestService;
        }

        [HttpPost]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> CreateRequest(CourseWorkRequestCreationDTO creationDTO)
        {
            if (creationDTO.StudentId == null)
            {
                creationDTO.StudentId = User.GetUserId();
            }
            await _courseWorkRequestService.Create(creationDTO);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CourseWorkRequestDTO>), 200)]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult<List<CourseWorkRequestDTO>>> GetAllByStatus(CourseWorkRequestStatus? status)
        {
            var requests = await _courseWorkRequestService.GetAllByStatus(status);
            return Ok(requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CourseWorkRequestDTO), 200)]
        [Route("{id:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<CourseWorkRequestDTO>> GetRequest(Guid id)
        {
            var request = await _courseWorkRequestService.GetById(id);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CourseWorkRequestDTO>), 200)]
        [Route("everything/{studentId:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<CourseWorkRequestDTO>>> GetAllByStudentId(Guid studentId)
        {
            var requests = await _courseWorkRequestService.GetByStudentId(studentId);
            return Ok(requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CourseWorkRequestDTO>), 200)]
        [Route("my")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<CourseWorkRequestDTO>>> GetMyRequests()
        {
            var studentId = User.GetUserId();
            var requests = await _courseWorkRequestService.GetByStudentId(studentId);
            return Ok(requests);
        }


        [HttpPost]
        [Route("{id:guid}/status")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> ChangeStatus(Guid id, CourseWorkRequestStatus newStatus)
        {
            await _courseWorkRequestService.ChangeStatus(
                id,
                newStatus,
                User.GetUserId(),
                User.Claims.Where(x => x.Type == ClaimTypes.Role).ToList()
                .All(r => r.Value != ApplicationRoleNames.Staff && r.Value != ApplicationRoleNames.Administrator));
            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> DeleteEmployment(Guid id)
        {
            await _courseWorkRequestService.Delete(id);
            return Ok();
        }
    }
}
