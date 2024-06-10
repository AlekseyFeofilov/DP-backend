namespace DP_backend.Common.EntityType;

public sealed class EntityType
{
    public required string Id { get; init; }

    public string? Description { get; set; }

    public EntityTypeUsage Usage { get; set; }
}

[Flags]
public enum EntityTypeUsage
{
    LinkFile = 0b01,
    LinkComment = 0b10
}