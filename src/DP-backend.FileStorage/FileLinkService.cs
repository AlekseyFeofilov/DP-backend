using DP_backend.Common.EntityType;
using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace DP_backend.FileStorage;

public interface IFileLinkService
{
    Task LinkFileToEntity(string entityType, string entityId, Guid fileId, Guid userId, CancellationToken ct);
    Task DetachFileToEntity(string entityType, string entityId, Guid fileId, CancellationToken ct);
    Task<IEnumerable<FileHandle>> GetLinkedFiles(string entityType, string entityId, CancellationToken ct);
}

internal class FileLinkService : IFileLinkService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEntityTypesProvider _entityTypesProvider;

    public FileLinkService(ApplicationDbContext dbContext, IEntityTypesProvider entityTypesProvider)
    {
        _dbContext = dbContext;
        _entityTypesProvider = entityTypesProvider;
    }

    public async Task LinkFileToEntity(string entityType, string entityId, Guid fileId, Guid userId, CancellationToken ct)
    {
        var fileHandle = await _dbContext.FileHandles.FindAsync([ fileId ], ct);
        if (fileHandle == null) throw new NotFoundException($"Файл {fileId} не найден");

        if (await _entityTypesProvider.ValidateEntityType(entityType, EntityTypeUsage.LinkFile, ct) == false)
        {
            throw new BadDataException($"Invalid entity type \"{entityType}\" to link file");
        }

        var fileEntityLink = new FileEntityLink { EntityType = entityType, EntityId = entityId, FileId = fileId, CreatedBy = userId };
        _dbContext.Add(fileEntityLink);
        fileHandle.LinksChangedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DetachFileToEntity(string entityType, string entityId, Guid fileId, CancellationToken ct)
    {
        var entityLink = await _dbContext.FileEntityLinks
            .Include(x => x.File)
            .FirstOrDefaultAsync(
                x => x.EntityType == entityType && x.EntityId == entityId && x.FileId == fileId,
                ct);
        if (entityLink == null) throw new NotFoundException($"Связь файла {fileId} с сущностью {entityType} {entityId} не найдена");

        _dbContext.Remove(entityLink);
        entityLink.File.LinksChangedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<FileHandle>> GetLinkedFiles(string entityType, string entityId, CancellationToken ct)
    {
        var fileHandles = await _dbContext.FileEntityLinks
            .Where(x => x.EntityType == entityType && x.EntityId == entityId)
            .Select(x => x.File)
            .AsNoTracking()
            .ToListAsync(ct);

        return fileHandles;
    }
}