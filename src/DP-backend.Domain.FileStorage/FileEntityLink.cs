namespace DP_backend.Domain.FileStorage;

public class FileEntityLink
{
    public required Guid FileId { get; init; }
    public FileHandle File { get; init; }

    /// <summary>
    /// Id from <see cref="DP_backend.Common.EntityType.EntityType"/>
    /// </summary>
    public required string EntityType { get; init; }
    public required string EntityId { get; init; }

    public Guid? CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}