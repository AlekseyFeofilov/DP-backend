using Microsoft.AspNetCore.Identity;
using static DP_backend.Models.IBaseEntitry;

namespace DP_backend.Models
{
    // Базовая моделька, возможно будем расширять
    public class Role : IdentityRole<Guid>, IBaseEntity
    {
        public ICollection<UserRole> Users { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }
}
