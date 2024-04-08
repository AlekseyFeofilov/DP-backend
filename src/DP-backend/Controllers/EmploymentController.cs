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
        [Route("{id:guid}")]
        [Authorize(Policy = $"{ApplicationRoleNames.Staff}, {ApplicationRoleNames.Student}")]
        public async Task<ActionResult<EmploymentDTO>> GetEmployment(Guid id)
        {
            var employment = await _employmentService.GetEmployment(
                id,
                User.GetUserId(),
                User.FindFirstValue(ApplicationRoleNames.Staff) == "true");
            return Ok(employment);
        }

        [HttpPost]
        
        public async Task<IActionResult> CreateEmployment(EmploymentСreationDTO employmentСreation, Guid UserId)
        {
            await _employmentService.CreateEmployment(UserId, employmentСreation);
            return Ok();
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Policy = $"{ApplicationRoleNames.Staff}, {ApplicationRoleNames.Student}")]
        public async Task<IActionResult> ChangeEmployment(Guid id, EmploymentChangeDTO employmentChange)
        {
            await _employmentService.ChangeEmployment(
                id,
                employmentChange,
                User.GetUserId(),
                User.FindFirstValue(ApplicationRoleNames.Staff) == "true");
            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Policy = ApplicationRoleNames.Staff)]
        public async Task<IActionResult> DeleteEmployment(Guid id)
        {
            await _employmentService.DeleteEmployment(id);
            return Ok();
        }
    }
}
