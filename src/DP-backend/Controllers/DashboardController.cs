﻿using DP_backend.Common.Enumerations;
using DP_backend.Domain.Employment;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public DashboardController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> GetStudentsDashboard(int page, Grade? grade, int? group, StudentStatus? status, string? namePart)
        {
            var students = await _userManagementService.GetStudentsByFilters(page, grade, group, status, namePart);
            return Ok(students);
        }
    }
}
