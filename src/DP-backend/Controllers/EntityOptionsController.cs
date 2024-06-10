using DP_backend.Common.EntityType;
using Microsoft.AspNetCore.Mvc;

namespace DP_backend.Controllers;

[ApiController]
public class EntityOptionsController : ControllerBase
{
    public record EntityTypeViewDto(string Id, string? Description, EntityTypeUsage Usage, string UsageDescription);

    [HttpGet("/api/entity/type/list")]
    public async Task<IEnumerable<EntityTypeViewDto>> GetEntityTypes([FromServices] IEntityTypesProvider entityTypesProvider, CancellationToken ct)
        => (await entityTypesProvider.GetEntityTypes(ct))
            .Values
            .Select(x => new EntityTypeViewDto(x.Id, x.Description, x.Usage, x.Usage.ToString()));
}