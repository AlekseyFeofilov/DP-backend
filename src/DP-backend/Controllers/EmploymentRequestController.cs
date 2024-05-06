using DP_backend.Domain.Employment;
using DP_backend.Helpers;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentRequestController : ControllerBase
    {
        private readonly IEmploymentService _employmentService;

        public EmploymentRequestController(IEmploymentService employmentService)
        {
            _employmentService = employmentService;
        }

        [HttpPost]
        [Route("{internshipRequestId}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> CreateEmploymentRequest(Guid internshipRequestId)
        {
            await _employmentService.CreateEmploymentRequest(internshipRequestId, User.GetUserId());
            return Ok();
        }

        [HttpPut]
        [Route("{employmentRequestId}/Status")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> UpdateEmploymentRequestStatus(Guid employmentRequestId, EmploymentRequestStatus status)
        {
            await _employmentService.UpdateEmploymentRequestStatus(employmentRequestId, status);
            return Ok();
        }

        [HttpGet]
        [Route("My")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetYourEmploymentRequests()
        {
            var requests = await _employmentService.GetStudentRequests(User.GetUserId());
            return Ok(requests);
        }
        [HttpGet]
        [Route("{studentId}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> GetStudentEmploymentRequests(Guid studentId)
        {
            var requests = await _employmentService.GetStudentRequests(studentId);
            return Ok(requests);
        }

    }
}
