using System;
using System.IO;
using System.Text.RegularExpressions;
using FileManager.Exceptions;

namespace FileManager.Shared.Processing
{
    public static class PathProcessing
    {
        private static readonly Regex _directorySeparatorCharRegex = new($"[\\{Path.DirectorySeparatorChar}]+", RegexOptions.Singleline);

        public static string NormalizaPath(string path = Constants.PathConstants.BaseDirectorySeparatorChar)
        {
            path ??= Constants.PathConstants.BaseDirectorySeparatorChar;
            path = path.Replace(Constants.PathConstants.BaseDirectorySeparatorChar, Path.DirectorySeparatorChar.ToString())
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            path = _directorySeparatorCharRegex.Replace(path, Path.DirectorySeparatorChar.ToString());

            if (path.IndexOf(Path.DirectorySeparatorChar) != 0)
            {
                path = Path.DirectorySeparatorChar + path;
            }

            return path;
        }

        public static string ValidatePath(string path, string basePath = null, string defaultBasePath = null, bool check = true)
        {
            if (path.Contains("..", StringComparison.OrdinalIgnoreCase))
            {
                throw new ListOutsideBasePathException();
            }

            var newBasePath = basePath ?? defaultBasePath;
            if (string.IsNullOrWhiteSpace(newBasePath))
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            var fullPath = $"{newBasePath}{path}";
            if (check && !Directory.Exists(fullPath))
            {
                throw new DirectoryDoesNotExistsException();
            }

            return fullPath;
        }
    }
}
