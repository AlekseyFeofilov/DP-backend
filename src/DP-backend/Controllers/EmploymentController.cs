using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentController : ControllerBase
    {
        private readonly IEmploymentService _employmentService;
        
        public EmploymentController(IEmploymentService employmentService)
        {
            _employmentService = employmentService;
        }

        [HttpGet]
        [Authorize(Policy = "EmploymentsRead")]
        public async Task<ActionResult<List<EmploymentDTO>>> GetEmployments(
            string? companyName = null, EmploymentStatus? employmentStatus = null)
        {
            var employments = await _employmentService.GetEmployments(companyName, employmentStatus);
            return Ok(employments);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Policy = "EmploymentControl")]
        public async Task<ActionResult<EmploymentDTO>> GetEmployment(Guid id)
        {
            var employment = await _employmentService.GetEmployment(
                id,
                User.GetUserId(),
                User.FindFirstValue(ApplicationRoleNames.Staff) == "true" || User.FindFirstValue(ApplicationRoleNames.Administrator) == "true");
            return Ok(employment);
        }

        [HttpPost]
        [Route("InternshipRequest")]
        [Authorize(Policy = $"EmploymentControl")]
        public async Task<IActionResult> CreateInternshipRequest(InternshipRequestСreationDTO employmentСreation)
        {
            await _employmentService.CreateInternshipRequest(User.GetUserId(), employmentСreation);
            return Ok();
        }

        [HttpPost]
        [Route("InternshipRequest/{internshipRequestId}/UpdateStatus")]
        [Authorize(Policy = $"EmploymentsRead")]
        public async Task<IActionResult> UpdateInternshipRequestStatus(Guid internshipRequestId, InternshipStatus status)
        {
            await _employmentService.UpdateInternshipRequestStatus(internshipRequestId, status);
            return Ok();
        }

        [HttpGet]
        [Route("InternshipRequest/My")]
        [Authorize(Policy = $"EmploymentControl")]
        public async Task<IActionResult> GetYourInternshipRequests()
        {
            var internshipRequests = await _employmentService.GetStudentInternshipRequests(User.GetUserId());
            return Ok(internshipRequests);
        }

        [HttpGet]
        [Route("InternshipRequest/{userId}")]
        [Authorize(Policy = $"EmploymentsRead")]
        public async Task<IActionResult> GetStudentInternshipRequests(Guid userId)
        {
            var internshipRequests = await _employmentService.GetStudentInternshipRequests(userId);
            return Ok(internshipRequests);
        }

        [HttpGet]
        [Route("InternshipRequest/NonVerified")]
        [Authorize(Policy = $"EmploymentsRead")]
        public async Task<IActionResult> GetNonVerifiedInternshipRequests()
        {
            var internshipRequests = await _employmentService.GetNonVerifiedInternshipRequests();
            return Ok(internshipRequests);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Policy = "EmploymentControl")]
        public async Task<IActionResult> ChangeEmployment(Guid id, EmploymentChangeDTO employmentChange)
        {
            await _employmentService.ChangeEmployment(
                id,
                employmentChange,
                User.GetUserId(),
                User.FindFirstValue(ApplicationRoleNames.Staff) == "true" || User.FindFirstValue(ApplicationRoleNames.Administrator) == "true");
            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Policy = "EmploymentDelete")]
        public async Task<IActionResult> DeleteEmployment(Guid id)
        {
            await _employmentService.DeleteEmployment(id);
            return Ok();
        }

        [HttpPost]
        [Route("EmploymentRequest/{internshipRequestId}")]
        [Authorize(Policy = "EmploymentControl")]
        public async Task<IActionResult> CreateEmploymentRequest(Guid internshipRequestId)
        {
            await _employmentService.CreateEmploymentRequest(internshipRequestId, User.GetUserId());
            return Ok();
        }

        [HttpPut]
        [Route("EmploymentRequest/{employmentRequestId}/StatusUpdate")]
        [Authorize(Policy = "EmploymentsRead")]
        public async Task<IActionResult> UpdateEmploymentRequestStatus(Guid employmentRequestId, EmploymentRequestStatus status)
        {
            await _employmentService.UpdateEmploymentRequestStatus(employmentRequestId, status);
            return Ok();
        }

        [HttpGet]
        [Route("Requests/My")]
        [Authorize(Policy = "EmploymentsRead")]
        public async Task<IActionResult> GetYourEmploymentRequests()
        {
            var requests = await _employmentService.GetStudentRequests(User.GetUserId());
            return Ok(requests);
        }
        [HttpGet]
        [Route("EmploymentRequest/{studentId}")]
        [Authorize(Policy = "EmploymentsRead")]
        public async Task<IActionResult> GetStudentEmploymentRequests(Guid studentId)
        {
           var requests = await _employmentService.GetStudentRequests(studentId);
            return Ok(requests);
        }
    }
}
