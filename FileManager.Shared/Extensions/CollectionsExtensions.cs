using System.Collections.Generic;
using System.Linq;

namespace FileManager.Shared.Extensions
{
    public static class CollectionsExtensions
    {
        public static IEnumerable<T> UnionAll<T>(this IEnumerable<IEnumerable<T>> lists) =>
            lists.Aggregate(Enumerable.Empty<T>(), (union, e) => union.Union(e));
    }
}
