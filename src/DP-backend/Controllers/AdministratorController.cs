using DP_backend.Domain.Identity;
using DP_backend.Models;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministrationService _administrationService;
        public AdministratorController(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }
        [Route("GiveRole/{userId}")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(Guid userId, ApplicationRoles role)
        {
            try
            {
                await  _administrationService.ChangeUserRole(userId, role);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(statusCode: 404, detail:ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500,detail: ex.Message);
            }
        }
    }
}
