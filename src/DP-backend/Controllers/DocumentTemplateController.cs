using DP_backend.Domain.Templating.Employment;
using DP_backend.Templating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentTemplateController : ControllerBase
{
    private readonly IDocumentTemplatesService _documentTemplatesService;
    private readonly IGenerateDocxInternshipDiaryUseCase _generateDocxInternshipDiaryUseCase;

    public DocumentTemplateController(IDocumentTemplatesService documentTemplatesService, IGenerateDocxInternshipDiaryUseCase generateDocxInternshipDiaryUseCase)
    {
        _documentTemplatesService = documentTemplatesService;
        _generateDocxInternshipDiaryUseCase = generateDocxInternshipDiaryUseCase;
    }

    public record CreateInternshipDiarySemester5Request(Guid TemplateFileId, IEnumerable<string> TemplateFieldIds, InternshipDiaryTemplate.Semester5PredefinedContext PredefinedContext);
    
    /// <returns>Id нового шаблона</returns>
    [Authorize(Roles = "Staff")]
    [HttpPost("InternshipDiary/semester/5")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateInternshipDiarySemester5(CreateInternshipDiarySemester5Request request, CancellationToken ct)
    {
        // var userId = User.GetUserId();
        var userId = Guid.Empty;

        var documentTemplate = InternshipDiaryTemplate.CreateFor5Semester(request.TemplateFileId, request.TemplateFieldIds, request.PredefinedContext);
        await _documentTemplatesService.AddDocumentTemplate(documentTemplate, userId, ct);

        return Created(null as string, documentTemplate.Id);
    }

    public record GenerateInternshipDiaryRequest(Guid InternshipDiaryRequestId, IEnumerable<InternshipDiaryTask> InternshipDiaryTasks, InternshipDiaryAssessment InternshipDiaryAssessment);

    /// <summary>
    /// Генерирует дневник и привязывает созданный файл к заявке на дневник практики 
    /// </summary>
    /// <param name="semester"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns>Id сгенерированного файла</returns>
    [Authorize(Policy = "StaffAndStudent")]
    [HttpPost("InternshipDiary/semester/{semester}/generate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> GenerateInternshipDiarySemester5([FromRoute] InternshipSemesters semester, GenerateInternshipDiaryRequest request, CancellationToken ct)
    {
        var file = await _generateDocxInternshipDiaryUseCase.Execute(
            InternshipDiaryTemplate.TypeBySemester(semester),
            request.InternshipDiaryRequestId,
            request.InternshipDiaryAssessment,
            request.InternshipDiaryTasks, ct);

        return Created(null as string, file.Id);
    }
}