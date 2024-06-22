using DP_backend.Common;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.FileStorage;
using DP_backend.Domain.Templating;
using DP_backend.Domain.Templating.Employment;
using DP_backend.FileStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EntityTypeIds = DP_backend.Domain.Employment.EntityTypeIds;

namespace DP_backend.Templating;

public interface IGenerateDocxInternshipDiaryUseCase
{
    Task<FileHandle> Execute(string templateType, Guid internshipDiaryRequestId, InternshipDiaryAssessment internshipDiaryAssessment, IEnumerable<InternshipDiaryTask> internshipDiaryTasks, CancellationToken ct);
}

public class GenerateDocxInternshipDiaryUseCase : IGenerateDocxInternshipDiaryUseCase
{
    private readonly IEnumerable<ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>> _templateFieldsResolvers;
    private readonly ApplicationDbContext _dbContext;
    private readonly IObjectStorageService _storageService;
    private readonly IFileLinkService _fileLinkService;
    private readonly ILogger<GenerateDocxInternshipDiaryUseCase> _logger;

    public GenerateDocxInternshipDiaryUseCase(
        [FromKeyedServices(nameof(InternshipDiaryTemplate))] IEnumerable<ITemplateFieldsResolver<InternshipDiaryTemplateResolutionContext>> templateFieldsResolvers,
        ApplicationDbContext dbContext,
        IObjectStorageService storageService,
        IFileLinkService fileLinkService,
        ILogger<GenerateDocxInternshipDiaryUseCase> logger)
    {
        _templateFieldsResolvers = templateFieldsResolvers;
        _dbContext = dbContext;
        _storageService = storageService;
        _fileLinkService = fileLinkService;
        _logger = logger;
    }

    public const string DocContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    public async Task<FileHandle> Execute(string templateType, Guid internshipDiaryRequestId, InternshipDiaryAssessment internshipDiaryAssessment, IEnumerable<InternshipDiaryTask> internshipDiaryTasks, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        // берём последний созданный шаблон подходящего типа 
        var template = await _dbContext.Set<DocumentTemplate>().GetUndeleted()
            .AsNoTracking()
            .Where(x => x.TemplateType == templateType)
            .OrderByDescending(x => x.CreateDateTime)
            .LastOrDefaultAsync(ct);

        if (template == null) throw new NotFoundException(templateType);

        var internshipDiaryRequest = await _dbContext.InternshipDiaryRequests.GetUndeleted()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == internshipDiaryRequestId, ct);

        if (internshipDiaryRequest == null) throw new NotFoundException(internshipDiaryRequestId.ToString());

        var templateResolutionContext = new InternshipDiaryTemplateResolutionContext
        {
            InternshipDiaryRequest = internshipDiaryRequest,
            InternshipDiaryAssessment = internshipDiaryAssessment,
            InternshipDiaryTasks = internshipDiaryTasks
        };

        try
        {
            return await CreateDocument(template, templateResolutionContext, now, ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "While generating internship diary {TemplateType} for internship diary request {InternshipDiaryRequestId}", templateType, internshipDiaryRequestId);
            throw;
        }
    }

    private async Task<FileHandle> CreateDocument(DocumentTemplate template, InternshipDiaryTemplateResolutionContext templateResolutionContext, DateTime now, CancellationToken ct)
    {

        var templateContextBuilder = new TemplateContextBuilder<InternshipDiaryTemplateResolutionContext>(templateResolutionContext);

        templateContextBuilder = _templateFieldsResolvers.Aggregate(
            templateContextBuilder,
            (builder, resolver) => builder.AddFieldsResolver(resolver));

        var templateContext = await templateContextBuilder.BuildForTemplate(template, ct);


        using var templateStream = new MemoryStream();

        await _storageService.DownloadFile(template.TemplateFileId, async (stream, handle, token) =>
        {
            try
            {
                await stream.CopyToAsync(templateStream, token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "While reading template {TemplateId}", template.Id);
                throw;
            }
        }, ct);

        templateStream.Seek(0, SeekOrigin.Begin);
        new DocxTemplateEvaluator().Evaluate(templateStream, templateContext);

        await using (var tx = await _dbContext.Database.BeginTransactionAsync(ct))
        {
            templateStream.Seek(0, SeekOrigin.Begin);
            var file = await _storageService.UploadFile(
                $"{template.TemplateType}_{now:yyyy_MM_dd_hh_mm}_UTC.doc",
                DocContentType,
                templateStream,
                templateResolutionContext.InternshipDiaryRequest.StudentId, ct);

            await _fileLinkService.LinkFileToEntity(
                EntityTypeIds.InternshipDiaryRequest,
                templateResolutionContext.InternshipDiaryRequest.Id.ToString(),
                file.Id,
                templateResolutionContext.InternshipDiaryRequest.StudentId, ct);

            await tx.CommitAsync(ct);
            return file;
        }
    }
}