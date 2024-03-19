using DP_backend.Common;

namespace DP_backend.Domain.Employment;

public class Comment : BaseEntity
{
    /// <summary>
    /// Пользователь отправитель
    /// </summary>
    public required Guid CreatedBy { get; init; }

    /// <summary>
    /// Сущность к которой привязан комментарий
    /// </summary>
    public required Guid TargetEntityId { get; init; }

    public required string Message { get; set; }
}