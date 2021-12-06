using System.Linq.Expressions;
using LiteDB;

namespace SyncMe.DataAccess.Extensions;

public static class LiteQueryableExtensions
{
    public static ILiteQueryable<T> ApplyFilter<T, TFilter>(
        this ILiteQueryable<T> queryable,
        TFilter? optionalFilter,
        Func<TFilter, Expression<Func<T, bool>>> predicate)
    {
        if (optionalFilter is null)
            return queryable;

        return queryable.Where(predicate.Invoke(optionalFilter));
    }
}
