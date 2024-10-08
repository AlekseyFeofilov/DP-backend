﻿using DP_backend.Common.Enumerations;
using DP_backend.Domain.Identity;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static DP_backend.Controllers.FileController;


namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Staff")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [Route("CreateGroup/{groupNumber}/{grade}")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup(int groupNumber, Grade grade)
        {
            try
            {
                await _groupService.CreateGroup(groupNumber, grade);
                return Ok();
            }

            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [Route("{groupId}/Delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(Guid groupId)
        {
            try
            {
                await _groupService.DeleteGroup(groupId);
                return Ok();
            }

            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GroupDTO>), 200)]
        public async Task<IActionResult> GetGroups(Grade? grade)
        {
            try
            {
               var groups = await _groupService.GetGroups(grade);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }

        [Route("{groupId}/Update")]
        [HttpPut]
        public async Task<IActionResult> UpdateGroup(Guid groupId,Grade grade)
        {
            try
            {
                await _groupService.ChangeGroupGrade(groupId, grade);
                return Ok();
            }

            catch (Exception ex)
            {
                return Problem(statusCode: 500, detail: ex.Message);
            }
        }
    }
}
