using DP_backend.Common.Exceptions;
using DP_backend.Database;
using DP_backend.Domain.FileStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace DP_backend.FileStorage;

public interface IObjectStorageService
{
    Task<BucketHandle> CreateBucket(CancellationToken ct = default);

    Task<FileHandle> UploadFile(string fileName, string contentType, Stream stream, Guid userId, CancellationToken ct = default);

    delegate Task DownloadFileCallback(Stream fileStream, FileHandle fileHandle, CancellationToken cancellationToken);

    Task DownloadFile(Guid fileId, DownloadFileCallback downloadCallback, CancellationToken ct = default);
}

internal class MinioStorageService : IObjectStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MinioStorageService> _logger;
    private readonly IOptionsMonitor<FileStorageConfiguration> _options;

    public MinioStorageService(IMinioClient minioClient, ApplicationDbContext dbContext, ILogger<MinioStorageService> logger, IOptionsMonitor<FileStorageConfiguration> options)
    {
        _minioClient = minioClient;
        _dbContext = dbContext;
        _logger = logger;
        _options = options;
    }

    public async Task<BucketHandle> CreateBucket(CancellationToken ct = default)
    {
        await using var tx = await _dbContext.Database.BeginTransactionAsync(ct);
        var bucketHandle = new BucketHandle();

        _dbContext.Add(bucketHandle);
        await _dbContext.SaveChangesAsync(ct);

        SetBucketName(bucketHandle);
        await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketHandle.BucketName), ct);
        await _dbContext.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);
        _logger.LogInformation("Создан новый бакет {BucketName}", bucketHandle.BucketName);
        return bucketHandle;
    }

    public async Task<FileHandle> UploadFile(string fileName, string contentType, Stream stream, Guid userId, CancellationToken ct = default)
    {
        var bucket = await _dbContext.BucketHandles.FirstOrDefaultAsync(x => x.State == BucketState.Writeable, ct);
        if (bucket == null)
        {
            _logger.LogWarning("Не найдено доступных бакетов для записи, содаётся новый");
            bucket = await CreateBucket(ct);
        }

        await using var tx = await _dbContext.Database.BeginTransactionAsync(ct);
        var fileHandle = await FileHandle.CreateWithStream(fileName, contentType, stream, bucket, userId, ct);

        stream.Seek(0, SeekOrigin.Begin);
        var putObject = new PutObjectArgs()
                .WithBucket(bucket.BucketName)
                .WithObject(fileHandle.GetObjectId())
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(fileHandle.ContentType);
        var response = await _minioClient.PutObjectAsync(putObject, ct);
        
        _dbContext.Add(fileHandle);
        await _dbContext.SaveChangesAsync(ct);

        await IncrementBucketStats(bucket, fileHandle, ct);
        await tx.CommitAsync(ct);

        return fileHandle;
    }

    public async Task DownloadFile(Guid fileId, IObjectStorageService.DownloadFileCallback fileCallback, CancellationToken ct = default)
    {
        var fileHandle = await _dbContext.FileHandles.FindAsync([ fileId ], ct);
        if (fileHandle == null) throw new NotFoundException($"Файл {fileId} не найден ");

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(fileHandle.Bucket.BucketName)
            .WithObject(fileHandle.GetObjectId())
            // cringe minio api
            .WithCallbackStream((stream, cancellationToken) => fileCallback.Invoke(stream, fileHandle, cancellationToken));

        var @object = await _minioClient.GetObjectAsync(getObjectArgs, ct);
    }

    private void SetBucketName(BucketHandle bucketHandle)
    {
        if (bucketHandle.Id == default) throw new ArgumentException("Id бакета должен быть инициализирован", nameof(bucketHandle.Id));
        bucketHandle.BucketName = $"{_options.CurrentValue.BucketPrefix}-{bucketHandle.Id}";
    }

    private Task IncrementBucketStats(BucketHandle bucket, FileHandle file, CancellationToken ct)
    {
        _dbContext.Entry(bucket).State = EntityState.Detached;
        return _dbContext.BucketHandles.Where(x => x.Id == bucket.Id)
            .ExecuteUpdateAsync(calls => calls
                    .SetProperty(x => x.ObjectsCount, x => x.ObjectsCount + 1)
                    .SetProperty(x => x.ObjectsSize, x => x.ObjectsSize + file.Size),
                ct);
    }
}