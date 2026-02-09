using Common.Contract;
using System.Linq.Dynamic.Core;

namespace Core.DB
{
    public static class LinqExtensions
    {
        public static IQueryable<T> ApplyQueryParameters<T>(this IQueryable<T> query, QueryParameters parameters)
        {
           if (!string.IsNullOrWhiteSpace(parameters.Where))
            {
                query = query.Where(parameters.Where);
            }

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                query = query.OrderBy(parameters.OrderBy);
            }

            return query.Skip((parameters.Page - 1) * parameters.Limit).Take(parameters.Limit);
        }
    }
}
