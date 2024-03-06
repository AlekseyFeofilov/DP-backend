using Microsoft.AspNetCore.Identity;
using static DP_backend.Models.IBaseEntitry;

namespace DP_backend.Models
{
    // Базовая моделька, будем расширять
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
        public ICollection<UserRole> Roles { get; set; }
    }
}
