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
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>  GetAll()
        {
            try
            {
                var employers = await _employerService.GetAllEmployers();
                return Ok(employers);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
            
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var employer = await _employerService.GetEmployerById(id);
                return Ok(employer);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(statusCode: 404, detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmployerPostDTO model)
        {
            try
            {
                await _employerService.AddEmployer(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(Guid id, EmployerPostDTO model)
        {
            try
            {
                await _employerService.UpdateEmployer(id, model);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(statusCode: 404, detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _employerService.DeleteEmployer(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(statusCode: 404, detail: ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }
    }
}
