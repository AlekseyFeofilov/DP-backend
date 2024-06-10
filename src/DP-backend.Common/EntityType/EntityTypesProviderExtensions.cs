namespace DP_backend.Common.EntityType;

public static class EntityTypesProviderExtensions
{
    public static async Task<bool> ValidateEntityType(this IEntityTypesProvider entityTypesProvider, string entityTypeId, EntityTypeUsage usage, CancellationToken ct)
    {
        var entityTypes = await entityTypesProvider.GetEntityTypes(ct);
        return entityTypes.TryGetValue(entityTypeId, out var entityType) && entityType.Usage.HasFlag(usage);
    }
}