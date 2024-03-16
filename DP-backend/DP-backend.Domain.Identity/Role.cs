using System;
using System.Collections.Generic;
using DP_backend.Common;
using Microsoft.AspNetCore.Identity;

namespace DP_backend.Domain.Identity
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
