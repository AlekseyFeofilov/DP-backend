using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipRequestController : ControllerBase
    {

        private readonly IEmploymentService _employmentService;

        public InternshipRequestController(IEmploymentService employmentService)
        {
            _employmentService = employmentService;
        }

        [HttpGet]
        [Route("NonVerified")]
        [ProducesResponseType(typeof(List<InternshipRequestDTO>), 200)]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> GetNonVerifiedInternshipRequests()
        {
            var internshipRequests = await _employmentService.GetNonVerifiedInternshipRequests();
            return Ok(internshipRequests);
        }

        [HttpPost]
        [Authorize(Policy = $"Staff")]
        public async Task<IActionResult> CreateInternshipRequest(InternshipRequestСreationDTO employmentСreation)
        {
            await _employmentService.CreateInternshipRequest(User.GetUserId(), employmentСreation);
            return Ok();
        }

        [HttpPost]
        [Route("{internshipRequestId}/Status")]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> UpdateInternshipRequestStatus(Guid internshipRequestId, InternshipStatus status)
        {
            await _employmentService.UpdateInternshipRequestStatus(internshipRequestId, status);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipRequestDTO>), 200)]
        [Route("My")]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> GetYourInternshipRequests()
        {
            var internshipRequests = await _employmentService.GetStudentInternshipRequests(User.GetUserId());
            return Ok(internshipRequests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipRequestDTO>), 200)]
        [Route("Student/{userId}")]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> GetStudentInternshipRequests(Guid userId)
        {
            var internshipRequests = await _employmentService.GetStudentInternshipRequests(userId);
            return Ok(internshipRequests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(InternshipRequestDTO), 200)]
        [Route("{internshipRequestId}")]
        [Authorize(Policy = $"Staff")]
        public async Task<IActionResult> GetInternshipRequests(Guid internshipRequestId)
        {
            var internshipRequest = await _employmentService.GetInternshipRequest(internshipRequestId);
            return Ok(internshipRequest);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipRequestDTO>), 200)]
        [Route("")]
        [Authorize(Policy = $"Staff")]
        public async Task<IActionResult> GetAllInternshipRequests(int? group, InternshipStatus? status)
        {
            var internshipRequest = await _employmentService.GetInternshipRequestsWithFilters(group, status);
            return Ok(internshipRequest);
        }
    }
}
