using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DP_backend.Models.IBaseEntitry;

namespace DP_backend.Helpers
{
    public static class IQueriableHelper
    {
        public static IQueryable<TEntity> GetUndeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : class, IBaseEntity
        {
            return query.Where(e => e.DeleteDateTime == null);
        }
    }
}
