namespace DP_backend.Common.EntityType;

public interface IEntityTypesProvider
{
    Task<Dictionary<string, EntityType>> GetEntityTypes(CancellationToken ct = default);
}