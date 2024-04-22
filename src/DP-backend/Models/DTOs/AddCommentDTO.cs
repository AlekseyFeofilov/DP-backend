namespace DP_backend.Models.DTOs
{
    public class AddCommentDTO
    {
        public Guid EntityId { get; set; }

        public string Message { get; set; }
    }
}
