using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class FileDTO
    {
        public Guid FileId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
