using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManager.Shared.Extensions
{
    public static class ThreadsExtensions
    {
        public static async Task<IEnumerable<T>> UnionWhenAll<T>(this IEnumerable<Task<IEnumerable<T>>> tasks)
        {
            var results = await Task.WhenAll(tasks);
            return results.UnionAll();
        }
    }
}
