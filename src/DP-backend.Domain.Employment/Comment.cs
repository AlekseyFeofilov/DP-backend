using DP_backend.Common;
using DP_backend.Common.EntityType;

namespace DP_backend.Domain.Employment;

public class Comment : BaseEntity
{
    /// <summary>
    /// Пользователь отправитель
    /// </summary>
    public required Guid CreatedBy { get; init; }

    /// <summary>
    /// Id сущности к которой привязан комментарий
    /// </summary>
    public required string TargetEntityId { get; init; }
    /// <summary>
    /// Тип сущности к которой привязан комментарий
    /// </summary>
    /// <remarks><see cref="EntityType"/></remarks>
    public required string TargetEntityType { get; set; }

    public required string Message { get; set; }
}