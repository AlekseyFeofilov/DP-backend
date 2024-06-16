using DP_backend.Database;
using DP_backend.Domain.Templating;
using DP_backend.FileStorage;

namespace DP_backend.Templating;

public interface IDocumentTemplatesService
{
    Task AddDocumentTemplate(DocumentTemplate documentTemplate, Guid userId, CancellationToken ct);
}

public class DocumentTemplatesService : IDocumentTemplatesService
{
    private readonly IFileLinkService _fileLinkService;
    private readonly ApplicationDbContext _dbContext;

    public DocumentTemplatesService(IFileLinkService fileLinkService, ApplicationDbContext dbContext)
    {
        _fileLinkService = fileLinkService;
        _dbContext = dbContext;
    }

    public async Task AddDocumentTemplate(DocumentTemplate documentTemplate, Guid userId, CancellationToken ct)
    {
        await using var tx = await _dbContext.Database.BeginTransactionAsync(ct);

        _dbContext.Set<DocumentTemplate>().Add(documentTemplate);
        await _dbContext.SaveChangesAsync(ct);
        await _fileLinkService.LinkFileToEntity(
            entityType: Domain.Templating.EntityTypeIds.Template,
            entityId: documentTemplate.Id.ToString(),
            fileId: documentTemplate.TemplateFileId,
            userId, ct);

        await tx.CommitAsync(ct);
    }
}