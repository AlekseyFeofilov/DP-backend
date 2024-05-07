using System.Security.Cryptography;

namespace DP_backend.Domain.FileStorage;

public class FileHandle
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; }

    public byte[] Hash { get; private init; } // todo use for deduplication
    public long Size { get; private init; }
    public string ContentType { get; private init; }

    public Guid? CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? LinksChangedAt { get; set; }

    public BucketHandle Bucket { get; set; }
    public List<FileEntityLink> Links { get; set; }

    protected FileHandle()
    {
    }

    public string GetObjectId() => Id.ToString();

    // todo recognize content type 
    public static async Task<FileHandle> CreateWithStream(string name, string contentType, Stream stream, BucketHandle bucket, Guid? createdBy = null, CancellationToken ct = default)
    {
        using var md5 = MD5.Create();
        stream.Seek(0, SeekOrigin.Begin);
        var hash = await md5.ComputeHashAsync(stream, ct);
        
        return new FileHandle
        {
            Name = name,
            Bucket = bucket,
            Hash = hash,
            Size = stream.Length,
            ContentType = contentType, // todo verify with magic bytes and list of allowed extensions
            CreatedBy = createdBy
        };
    }
}