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
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<EmploymentDTO>>> GetEmployments(
            string? companyName = null, EmploymentStatus? employmentStatus = null)
        {
            var employments = await _employmentService.GetEmployments(companyName, employmentStatus);
            return Ok(employments);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Policy = "Staff")]
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
        [Authorize(Policy = $"Staff")]
        public async Task<IActionResult> CreateInternshipRequest(InternshipRequestСreationDTO employmentСreation)
        {
            await _employmentService.CreateInternshipRequest(User.GetUserId(), employmentСreation);
            return Ok();
        }

        [HttpPost]
        [Route("InternshipRequest/{internshipRequestId}/UpdateStatus")]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> UpdateInternshipRequestStatus(Guid internshipRequestId, InternshipStatus status)
        {
            await _employmentService.UpdateInternshipRequestStatus(internshipRequestId, status);
            return Ok();
        }

        [HttpGet]
        [Route("InternshipRequest/My")]
        [Authorize(Policy = $"Staff")]
        public async Task<IActionResult> GetYourInternshipRequests()
        {
            var internshipRequests = await _employmentService.GetStudentInternshipRequests(User.GetUserId());
            return Ok(internshipRequests);
        }

        [HttpGet]
        [Route("InternshipRequest/{userId}")]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> GetStudentInternshipRequests(Guid userId)
        {
            var internshipRequests = await _employmentService.GetStudentInternshipRequests(userId);
            return Ok(internshipRequests);
        }

        [HttpGet]
        [Route("InternshipRequest/NonVerified")]
        [Authorize(Policy = $"StaffAndStudent")]
        public async Task<IActionResult> GetNonVerifiedInternshipRequests()
        {
            var internshipRequests = await _employmentService.GetNonVerifiedInternshipRequests();
            return Ok(internshipRequests);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Policy = "Staff")]
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
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> DeleteEmployment(Guid id)
        {
            await _employmentService.DeleteEmployment(id);
            return Ok();
        }

        [HttpPost]
        [Route("EmploymentRequest/{internshipRequestId}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateEmploymentRequest(Guid internshipRequestId)
        {
            await _employmentService.CreateEmploymentRequest(internshipRequestId, User.GetUserId());
            return Ok();
        }

        [HttpPut]
        [Route("EmploymentRequest/{employmentRequestId}/StatusUpdate")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> UpdateEmploymentRequestStatus(Guid employmentRequestId, EmploymentRequestStatus status)
        {
            await _employmentService.UpdateEmploymentRequestStatus(employmentRequestId, status);
            return Ok();
        }

        [HttpGet]
        [Route("Requests/My")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetYourEmploymentRequests()
        {
            var requests = await _employmentService.GetStudentRequests(User.GetUserId());
            return Ok(requests);
        }
        [HttpGet]
        [Route("EmploymentRequest/{studentId}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetStudentEmploymentRequests(Guid studentId)
        {
           var requests = await _employmentService.GetStudentRequests(studentId);
            return Ok(requests);
        }

        [HttpGet]
        [Route("My")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetYourEmployments()
        {
            var employments = await _employmentService.GetStudentEmployments(User.GetUserId());
            return Ok(employments);
        }
        [HttpGet]
        [Route("{studentId}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetStudentEmployments(Guid studentId)
        {
            var employments = await _employmentService.GetStudentEmployments(studentId);
            return Ok(employments);
        }

        [HttpGet]
        [Route("Everything/My")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetYourEmploymentsInfo()
        {
            var employmentsInfo = await _employmentService.GetAllStudentEmploymentInformation(User.GetUserId());
            return Ok(employmentsInfo);
        }

        [HttpGet]
        [Route("Everything/{studentId}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetStudentEmploymentsInfo(Guid studentId)
        {
            var employmentsInfo = await _employmentService.GetAllStudentEmploymentInformation(studentId);
            return Ok(employmentsInfo);
        }
    }
}
