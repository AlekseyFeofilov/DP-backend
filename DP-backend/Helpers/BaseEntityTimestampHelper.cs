using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DP_backend.Models.IBaseEntitry;


namespace DP_backend.Helpers
{
    internal static class BaseEntityTimestampHelper
    {
        internal static void SetTimestamps(ChangeTracker changeTracker, DateTime? dateTime = null)
        {
            if (dateTime.HasValue && dateTime.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("DateTime stored in database must have DateTime.Kind=UTC");
            }

            var entities = changeTracker.Entries<IBaseEntity>();
            var now = dateTime ?? DateTime.UtcNow;

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreateDateTime = now;
                        entity.Entity.ModifyDateTime = now;
                        break;
                    case EntityState.Modified:
                        entity.Entity.ModifyDateTime = now;
                        break;
                    case EntityState.Deleted:
                        entity.Entity.DeleteDateTime = now;
                        entity.State = EntityState.Modified;
                        break;
                }
            }
        }
    }

}
