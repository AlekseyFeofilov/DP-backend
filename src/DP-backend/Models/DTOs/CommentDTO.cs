using DP_backend.Domain.Employment;
using DP_backend.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }

        [Required]
        public UserDTO Author { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }

        public CommentDTO() { }

        public CommentDTO(Comment comment, User user) 
        {
            Id = comment.Id;
            Author = new UserDTO(user);
            Message = comment.Message;
            CreateDateTime = comment.CreateDateTime;
        }
    }
}
