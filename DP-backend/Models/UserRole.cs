using Microsoft.AspNetCore.Identity;
using System.Data;

namespace DP_backend.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
