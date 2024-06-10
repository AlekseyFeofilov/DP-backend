namespace DP_backend.Models.DTOs;

public class AddCommentDTO
{
    public required string EntityId { get; set; }
    /// <summary>
    /// Id from <see cref="EntityType"/>
    /// </summary>
    public required string EntityType { get; set; }
    public required string Message { get; set; }
}