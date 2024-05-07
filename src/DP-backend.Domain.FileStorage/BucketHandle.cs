using System.Text.Json.Serialization;

namespace DP_backend.Domain.FileStorage;

public class BucketHandle
{
    public int Id { get; init; }

    /// <remarks>Значение <c>null</c> соответствует не инициализированному бакету</remarks>
    public string? BucketName { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public BucketState State { get; set; } = BucketState.Writeable;

    public long ObjectsSize { get; set; } = 0;
    public long ObjectsCount { get; set; } = 0;
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BucketState
{
    Writeable = 1,
    Readonly = 2
}