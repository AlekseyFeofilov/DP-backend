using DP_backend.Domain.Identity;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    [Authorize(Policy = "Staff")]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }
        
        [HttpGet]
        [Route("GetAll")]
        [AllowAnonymous]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult>  GetAll(bool? isPartner)
        {
                var employers = await _employerService.GetAllEmployers(isPartner);
                return Ok(employers);            
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {

                var employer = await _employerService.GetEmployerById(id);
                return Ok(employer);

        }

        [HttpPost]
        public async Task<IActionResult> Post(EmployerPostDTO model)
        {
                await _employerService.AddEmployer(model);
                return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(Guid id, EmployerPostDTO model)
        {
                await _employerService.UpdateEmployer(id, model);
                return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
                await _employerService.DeleteEmployer(id);
                return Ok();
        }
    }
}
