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
    public class InternshipDiaryRequestController : ControllerBase
    {
        private readonly IInternshipDiaryRequestService _internshipDiaryRequestService;

        public InternshipDiaryRequestController(IInternshipDiaryRequestService internshipDiaryRequestService)
        {
            _internshipDiaryRequestService = internshipDiaryRequestService;
        }

        [HttpPost]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> CreateRequest(InternshipDiaryRequestCreationDTO creationDTO)
        {
            await _internshipDiaryRequestService.Create(creationDTO);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipDiaryRequestDTO>), 200)]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult<List<InternshipDiaryRequestDTO>>> GetAllByStatus(InternshipDiaryRequestStatus? status)
        {
            var requests = await _internshipDiaryRequestService.GetAllByStatus(status);
            return Ok(requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(InternshipDiaryRequestDTO), 200)]
        [Route("{id:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<InternshipDiaryRequestDTO>> GetRequest(Guid id)
        {
            var request = await _internshipDiaryRequestService.GetById(id);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipDiaryRequestDTO>), 200)]
        [Route("everything/{studentId:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<InternshipDiaryRequestDTO>>> GetAllByStudentId(Guid studentId)
        {
            var requests = await _internshipDiaryRequestService.GetByStudentId(studentId);
            return Ok(requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipDiaryRequestDTO>), 200)]
        [Route("my")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<InternshipDiaryRequestDTO>>> GetMyRequests()
        {
            var studentId = User.GetUserId();
            var requests = await _internshipDiaryRequestService.GetByStudentId(studentId);
            return Ok(requests);
        }


        [HttpPost]
        [Route("{id:guid}/status")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> ChangeStatus(Guid id, InternshipDiaryRequestStatus newStatus)
        {
            await _internshipDiaryRequestService.ChangeStatus(
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
            await _internshipDiaryRequestService.Delete(id);
            return Ok();
        }
    }
}
