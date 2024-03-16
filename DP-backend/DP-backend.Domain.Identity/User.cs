using System;
using System.Collections.Generic;
using DP_backend.Common;
using Microsoft.AspNetCore.Identity;

namespace DP_backend.Domain.Identity
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public Guid AccountId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
        public ICollection<UserRole> Roles { get; set; }
    }
}
