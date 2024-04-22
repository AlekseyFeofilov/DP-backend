using DP_backend.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace DP_backend.Models.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public UserDTO() { }

        public UserDTO(User user) 
        { 
            Id = user.Id;
            Name = user.UserName;
            Email = user.Email;
        }
    }
}
