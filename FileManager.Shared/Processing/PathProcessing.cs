using System;
using System.IO;
using System.Text.RegularExpressions;
using FileManager.Exceptions;
using FileManager.Shared.Constants;

namespace FileManager.Shared.Processing
{
    public static class PathProcessing
    {
        private static readonly Regex _directorySeparatorCharRegex = new($"[\\{Path.DirectorySeparatorChar}]+", RegexOptions.Singleline);

        public static string NormalizaPath(string path = PathConstants.BaseDirectorySeparatorChar)
        {
            path ??= PathConstants.BaseDirectorySeparatorChar;
            path = NormalizeSeparatorChar(path);

            if (path.IndexOf(Path.DirectorySeparatorChar) != 0)
            {
                path = Path.DirectorySeparatorChar + path;
            }

            return path;
        }

        public static string ComputeFullPath(string path = null, string basePath = null) => NormalizeSeparatorChar($"{basePath}{Path.DirectorySeparatorChar}{path}");

        public static void ValidateFilePath(string path, string basePath = null, string defaultBasePath = null, bool check = true)
        {
            ValidatePath(path, basePath, defaultBasePath);

            var newBasePath = ComputeBasePath(basePath, defaultBasePath);
            var fullPath = ComputeFullPath(path, newBasePath);
            if (check && !File.Exists(fullPath))
            {
                throw new FileNotFoundException();
            }
        }

        public static void ValidateBasePath(string path)
        {
            if (string.Equals(path, PathConstants.BaseDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                throw new OperationNotAllowedOnBasePathException();
            }
        }

        public static void ValidateDirectoryPath(string path, string basePath = null, string defaultBasePath = null, bool check = true)
        {
            ValidatePath(path, basePath, defaultBasePath);

            var newBasePath = ComputeBasePath(basePath, defaultBasePath);
            var fullPath = ComputeFullPath(path, newBasePath);
            if (check && !Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException();
            }
        }

        private static string NormalizeSeparatorChar(string path)
        {
            path = path.Replace(PathConstants.BaseDirectorySeparatorChar, Path.DirectorySeparatorChar.ToString())
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            path = _directorySeparatorCharRegex.Replace(path, Path.DirectorySeparatorChar.ToString());
            return path;
        }

        private static string ComputeBasePath(string basePath = null, string defaultBasePath = null) => basePath ?? defaultBasePath;

        private static void ValidatePath(string path, string basePath, string defaultBasePath)
        {
            if (path.Contains("..", StringComparison.OrdinalIgnoreCase))
            {
                throw new ListOutsideBasePathException();
            }

            if (string.IsNullOrWhiteSpace(basePath ?? defaultBasePath))
            {
                throw new ArgumentNullException(nameof(basePath));
            }
        }
    }
}
