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
