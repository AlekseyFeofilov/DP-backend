using DP_backend.Common.EntityType;
using DP_backend.FileStorage;
using DP_backend.Helpers;
using DP_backend.Models.DTOs;
using DP_backend.Services;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Отправка файла на сервер
    /// </summary>
    /// <param name="formFile"> файл в multipart/form-data </param>
    /// <param name="ct"></param>
    /// <returns>Id загруженного объекта</returns>
    [Authorize]
    [HttpPost]
    public async Task<Guid> UploadFile(IFormFile formFile, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var stream = formFile.OpenReadStream();

        var fileHandle = await _objectStorageService.UploadFile(formFile.FileName, formFile.ContentType, stream, userId, ct);

        return fileHandle.Id;
    }

    /// <summary>
    /// Загрузка файла с сервера
    /// </summary>
    /// <param name="fileId">Id ранее загруженного объекта</param>
    /// <param name="ct"></param>
    [Authorize]
    [HttpGet("{fileId}")]
    [ProducesResponseType(403)]
    [ProducesResponseType(typeof(string), 200)]
    public async Task DownloadFile(Guid fileId, CancellationToken ct)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, new FileCreatorOrStaff(fileId));
        if (!authorizationResult.Succeeded)
        {
            var forbidResult = Forbid("Нет доступа к файлу");
            await forbidResult.ExecuteResultAsync(ControllerContext);
            return;
        }

        await _objectStorageService.DownloadFile(
            fileId,
            (stream, fileHandle, cancellationToken) =>
            {
                var fileStreamResult = File(stream, fileHandle.ContentType, fileHandle.Name);
                return fileStreamResult.ExecuteResultAsync(ControllerContext);
            },
            ct);
    }

    /// <summary>
    /// Прикрепить файл к сущности
    /// </summary>
    /// <param name="fileId">Id ранее загруженного объекта</param>
    /// <param name="entityType">Тип сущности \ имя сущности \ пространство имён; Id from <see cref="EntityType"/></param>
    /// <param name="entityId">Id сущности</param>
    /// <param name="ct"></param>
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

    /// <summary>
    /// Открепить файл от сущности
    /// </summary>
    /// <param name="fileId">Id ранее загруженного объекта</param>
    /// <param name="entityType">Тип сущности \ имя сущности \ пространство имён; Id from <see cref="EntityType"/></param>
    /// <param name="entityId">Id сущности</param>
    /// <param name="ct"></param>
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

    public record EntityFilesResponse(string EntityType, string EntityId, IEnumerable<FileDTO> Files);

    // todo maybe some rule checks 
    /// <summary>
    /// Получить информацию о файлах прикрепленных к сущности 
    /// </summary>
    /// <param name="entityType">Тип сущности \ имя сущности \ пространство имён; Id from <see cref="EntityType"/></param>
    /// <param name="entityId">Id сущности</param>
    /// <param name="ct"></param>
    [Authorize]
    [ProducesResponseType(typeof(EntityFilesResponse), 200)]
    [HttpGet("{entityType}/{entityId}")]
    public async Task<EntityFilesResponse> GetLinkedFiles(string entityType, string entityId, CancellationToken ct)
    {
        var linkedFiles = await _fileLinkService.GetLinkedFiles(entityType, entityId, ct);
        return new EntityFilesResponse(entityType, entityId,
            linkedFiles.Select(file => new FileDTO { FileId = file.Id, FileName = file.Name, ContentType = file.ContentType, Size = file.Size, CreatedAt = file.CreatedAt }));
    }
}