using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DP_backend.Domain.Templating.Employment;
using DP_backend.Templating;

namespace DP_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipDiaryRequestController : ControllerBase
    {
        private readonly IInternshipDiaryRequestService _internshipDiaryRequestService;

        public InternshipDiaryRequestController(IInternshipDiaryRequestService internshipDiaryRequestService)
        {
            _internshipDiaryRequestService = internshipDiaryRequestService;
        }

        [HttpPost]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> CreateRequest(InternshipDiaryRequestCreationDTO creationDTO, CancellationToken ct)
        {
            if (creationDTO.StudentId == null)
            {
                creationDTO.StudentId = User.GetUserId();
            }

            var internshipDiaryRequest = await _internshipDiaryRequestService.Create(creationDTO);

            await GenerateInternshipDiaryTemplate(internshipDiaryRequest, ct);

            return Ok();
        }

        // todo : remove
        [Obsolete("#2471")]
        private async Task GenerateInternshipDiaryTemplate(InternshipDiaryRequest internshipDiaryRequest, CancellationToken ct)
        {
            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<InternshipDiaryRequestController>>();
            if (internshipDiaryRequest.Semester != (int)InternshipSemesters.Firth)
            {
                logger.LogInformation("Шаблон дневника практики не был сгенерирован при создании заявки на дневник практики {InternshipDiaryRequestId} семестр {SemesterNumber} для студента {StudentId}", internshipDiaryRequest.Id,
                    internshipDiaryRequest.Semester, internshipDiaryRequest.StudentId);
                return;
            }

            try
            {
                var generateDocxInternshipDiaryUseCase = HttpContext.RequestServices.GetRequiredService<IGenerateDocxInternshipDiaryUseCase>();
                var fileHandle = await generateDocxInternshipDiaryUseCase.Execute(
                    InternshipDiaryTemplate.TypeBySemester(InternshipSemesters.Firth),
                    internshipDiaryRequest.Id,
                    new InternshipDiaryAssessment(
                        Mark: int.MinValue,
                        Text: $"""
                               ***
                               Заполните это поле текстом характеристики с места практики.
                               Заполните оценку практики [{int.MinValue}].
                               Заполните дату создания характеристики [{InternshipDiaryTemplate.Formatting.Date(DateTime.MinValue)}].
                               ***
                               """,
                        Date: DateTime.MinValue),
                    Array.Empty<InternshipDiaryTask>(),
                    ct);

                logger.LogInformation("Создан шаблон дневника практики {FileName} Id {FileId} для заявки {InternshipDiaryRequestId}", fileHandle.Name, fileHandle.Id, internshipDiaryRequest.Id);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при генерации шаблона дневника практики во время создании заявки на дневник практики {InternshipDiaryRequestId} семестр {SemesterNumber} для студента {StudentId}", internshipDiaryRequest.Id,
                    internshipDiaryRequest.Semester, internshipDiaryRequest.StudentId);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipDiaryRequestDTO>), 200)]
        [Authorize(Policy = "Staff")]
        public async Task<ActionResult<List<InternshipDiaryRequestDTO>>> GetAllByStatus(InternshipDiaryRequestStatus? status)
        {
            var requests = await _internshipDiaryRequestService.GetAllByStatus(status);
            return Ok(requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(InternshipDiaryRequestDTO), 200)]
        [Route("{id:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<InternshipDiaryRequestDTO>> GetRequest(Guid id)
        {
            var request = await _internshipDiaryRequestService.GetById(id);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipDiaryRequestDTO>), 200)]
        [Route("everything/{studentId:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<InternshipDiaryRequestDTO>>> GetAllByStudentId(Guid studentId)
        {
            var requests = await _internshipDiaryRequestService.GetByStudentId(studentId);
            return Ok(requests);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InternshipDiaryRequestDTO>), 200)]
        [Route("my")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<ActionResult<List<InternshipDiaryRequestDTO>>> GetMyRequests()
        {
            var studentId = User.GetUserId();
            var requests = await _internshipDiaryRequestService.GetByStudentId(studentId);
            return Ok(requests);
        }


        [HttpPost]
        [Route("{id:guid}/status")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> ChangeStatus(Guid id, InternshipDiaryRequestStatus newStatus)
        {
            await _internshipDiaryRequestService.ChangeStatus(
                id,
                newStatus,
                User.GetUserId(),
                User.Claims.Where(x => x.Type == ClaimTypes.Role).ToList()
                    .All(r => r.Value != ApplicationRoleNames.Staff && r.Value != ApplicationRoleNames.Administrator));
            return Ok();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Policy = "StaffAndStudent")]
        public async Task<IActionResult> DeleteEmployment(Guid id)
        {
            await _internshipDiaryRequestService.Delete(id);
            return Ok();
        }

        [HttpPut]
        [Route("{id:guid}/Mark/{mark:float}")]
        [Authorize(Policy = "Staff")]
        public async Task<IActionResult> SetGrade(Guid id, [Range(1, 5)] float mark)
        {
            await _internshipDiaryRequestService.SetGrade(id, mark);
            return Ok();
        }
    }
}