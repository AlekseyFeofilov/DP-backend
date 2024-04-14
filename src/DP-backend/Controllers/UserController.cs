using DP_backend.Common.Enumerations;
using DP_backend.Domain.Employment;
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
    [Authorize(Policy = "UserControl")]
    public class UserController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [Route("GiveRole/{userId}")]
        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(Guid userId, ApplicationRoles role)
        {
            try
            {
                await _userManagementService.ChangeUserRole(userId, role);
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

        [Route("{userId}/SetStudentGroup/{groupId}")]
        [HttpPost]
        public async Task<IActionResult> SetStudentGroup(Guid userId, Guid groupId)
        {
            try
            {
                await _userManagementService.SetStudentGroup(userId, groupId);
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

        [HttpGet]
        [Route("ByGroup")]
        public async Task<IActionResult> GetStudents(Grade? grade=null, int? groupNumber=null, bool withoutGroups=false)
        {
            try
            {
                var students = await _userManagementService.GetStudentsFromGroup(grade, groupNumber, withoutGroups);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [HttpGet]
        [Route("{studentId}/GetStatus")]
        public async Task<IActionResult> GetStudentStatus(Guid studentId)
        {
            try
            {
                var status = await _userManagementService.GetStudentStatus(studentId);
                return Ok(status);
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
        [HttpGet]
        [Route("GetWihStatus")]
        public async Task<IActionResult> GetStudentsWithStatuses(List<StudentStatus> statuses)
        {
            try
            {
                var students = await _userManagementService.GetStudentsWithStatuses(statuses);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

    }
}
