using DP_backend.Models.DTOs.TSUAccounts;
using DP_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ITSUAccountService _accountService;
        private readonly ApplicationDbContext _context;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IUserManagementService _userManagementService;
        public UserController(ITSUAccountService tSUAccountService, ApplicationDbContext context, IUserManagementService userManagementService, IJwtAuthService jwtAuthService) 
        { 

            _accountService = tSUAccountService;
            _context = context;
            _userManagementService = userManagementService;
            _jwtAuthService = jwtAuthService;
        }
        [HttpPost]
        [SwaggerResponse(400)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Auth(string token)
        {
            TSUAuthResponseDTO data = null;
            try
            {
                data = await _accountService.GetAuthData(token);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 501, detail: ex.Message);
            }
            var user = await _context.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.AccountId == data.AccountId);
            if (user == null)
            {
                try
                {
                    user = await _userManagementService.CreateUserByAccountId(data.AccountId);
                }
                catch (ArgumentException)
                {
                    return StatusCode(401);
                }
                catch (Exception e)
                {
                    return StatusCode(500);
                }
                if (user == null)
                {

                    return StatusCode(403);
                }

            }
            try
            {
                var jwtToken = await _jwtAuthService.GenerateToken(user);
                return Ok(jwtToken);
            }
            catch (InvalidOperationException ex)
            {

                return Problem(statusCode: 500, detail: ex.Message);
            }
        }
    }
}
