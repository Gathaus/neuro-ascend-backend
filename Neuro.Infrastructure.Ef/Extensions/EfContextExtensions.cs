using Microsoft.EntityFrameworkCore;

namespace Neuro.Infrastructure.Ef.Extensions
{
    public static class EntityFrameworkExtensions
    {
        #region Methods
        // https://www.thereformedprogrammer.net/ef-core-in-depth-soft-deleting-data-with-global-query-filters/
        // AddSoftDeleteQueryFilter will be added
        
        
            public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params string[] includeProperties)
                where T : class
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
                return queryable;
            }
        #endregion
    }
}
