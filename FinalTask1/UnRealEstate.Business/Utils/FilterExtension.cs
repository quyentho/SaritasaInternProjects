using System.Linq;
using System.Linq.Expressions;

namespace UnrealEstate.Business.Utils
{
    public static class FilterExtension
    {
        public static IQueryable<TSource> FilterByRange<TSource>(this IQueryable<TSource> source, int? offset, int? limit)
        {
            if (offset.HasValue)
            {
                source = source.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                source = source.Take(limit.Value);
            }

            return source;
        }

        public static IQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> source, string sortBy)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                var parameter = Expression.Parameter(typeof(TSource), "x");

                Expression property = Expression.Property(parameter, sortBy);

                var lambda = Expression.Lambda(property, parameter);

                var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
                var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
                var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

                return (IOrderedQueryable<TSource>)result;
            }

            return source;
        }
    }
}
