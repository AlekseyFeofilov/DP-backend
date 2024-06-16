namespace DP_backend.Common;

public static class QueryableHelper
{
    public static IQueryable<TEntity> GetUndeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : class, IBaseEntity
    {
        return query.Where(e => e.DeleteDateTime == null);
    }

    public static IQueryable<T> If<T>(this IQueryable<T> queryable, bool condition, Func<IQueryable<T>, IQueryable<T>> option)
        => !condition ? queryable : option(queryable);
}