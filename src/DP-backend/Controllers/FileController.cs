using DP_backend.Domain.FileStorage;
using DP_backend.FileStorage;
using DP_backend.Helpers;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IObjectStorageService _objectStorageService;
    private readonly IFileLinkService _fileLinkService;

    public FileController(IAuthorizationService authorizationService, IObjectStorageService objectStorageService, IFileLinkService fileLinkService)
    {
        _authorizationService = authorizationService;
        _objectStorageService = objectStorageService;
        _fileLinkService = fileLinkService;
    }

    [Authorize]
    [HttpPost]
    public async Task<Guid> UploadFile(IFormFile formFile, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var stream = formFile.OpenReadStream();

        var fileHandle = await _objectStorageService.UploadFile(formFile.FileName, stream, userId, ct);

        return fileHandle.Id;
    }

    [Authorize]
    [HttpGet("{fileId}/url")]
    [ProducesResponseType(403)]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> GetFileDownloadUrl(Guid fileId, CancellationToken ct)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new FileCreatorOrStaff(fileId));
        if (!authorizationResult.Succeeded) return Forbid("Нет доступа к файлу");

        var (fileUrl, contentType, fileName) = await _objectStorageService.DownloadFileThrewUrl(fileId, ct);

        return Ok(fileUrl);
    }

    [Authorize]
    [HttpGet("{fileId}")]
    [ProducesResponseType(403)]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> DownloadFile(Guid fileId, CancellationToken ct)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new FileCreatorOrStaff(fileId));
        if (!authorizationResult.Succeeded) return Forbid("Нет доступа к файлу");

        var (fileStream, contentType, fileName) = await _objectStorageService.DownloadFile(fileId, ct);

        return File(fileStream, contentType, fileName);
    }

    [Authorize]
    [HttpPut("{fileId}/link/{entityType}/{entityId}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> LinkFile(Guid fileId, string entityType, string entityId, CancellationToken ct)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new FileCreatorOrStaff(fileId));
        if (!authorizationResult.Succeeded) return Forbid("Нет доступа к файлу");

        var userId = User.GetUserId();
        await _fileLinkService.LinkFileToEntity(entityType, entityId, fileId, userId, ct);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{fileId}/link/{entityType}/{entityId}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DetachFile(Guid fileId, string entityType, string entityId, CancellationToken ct)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new FileCreatorOrStaff(fileId));
        if (!authorizationResult.Succeeded) return Forbid("Нет доступа к файлу");

        await _fileLinkService.DetachFileToEntity(entityType, entityId, fileId, ct);

        return NoContent();
    }

    public record EntityFilesResponse(string EntityType, string EntityId, IEnumerable<FileDto> Files);

    public record FileDto(Guid FileId, string FileName, long Size, DateTime CreatedAt);

    // todo some rule checks 
    [Authorize]
    [ProducesResponseType(typeof(EntityFilesResponse), 200)]
    [HttpGet("{entityType}/{entityId}")]
    public async Task<EntityFilesResponse> GetLinkedFiles(string entityType, string entityId, CancellationToken ct)
    {
        var linkedFiles = await _fileLinkService.GetLinkedFiles(entityType, entityId, ct);
        return new EntityFilesResponse(entityType, entityId,
            linkedFiles.Select(file => new FileDto(file.Id, file.Name, file.Size, file.CreatedAt)));
    }
}