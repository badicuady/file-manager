using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileManager.Shared.Extensions
{
    public static class IoExtensions
    {
        public static long GetDirectorySize(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            var startDirectorySize = default(long);
            if (directoryInfo == null || !directoryInfo.Exists)
            {
                return startDirectorySize;
            }

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                Interlocked.Add(ref startDirectorySize, fileInfo.Length);
            }

            if (recursive)
            {
                Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
                    Interlocked.Add(ref startDirectorySize, GetDirectorySize(subDirectory, recursive)));
            }

            return startDirectorySize;
        }
    }
}
