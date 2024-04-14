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
        [Authorize(Policy = $"{ApplicationRoleNames.Student}")]
        public async Task<IActionResult> CreateEmployment(EmploymentСreationDTO employmentСreation)
        {
            await _employmentService.CreateEmployment(User.GetUserId(), employmentСreation);
            return Ok();
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
    }
}
