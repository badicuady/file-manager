using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Application.Interfaces;
using FileManager.Exceptions;
using FileManager.Shared.Constants;
using FileManager.Shared.Processing;
using FileManager.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileManager.Application.Handlers.CommandHandlers
{
    public class CopyDirectoryHandler : ICopyDirectoryHandler
    {

        private readonly ILogger<CopyDirectoryHandler> _logger;
        private readonly ManagerSettings _settings;

        public CopyDirectoryHandler(ILogger<CopyDirectoryHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string copyDirectoryName = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            copyDirectoryName = PathProcessing.NormalizaPath(copyDirectoryName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var oldPath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var newPath = PathProcessing.ComputeFullPath(copyDirectoryName, _settings.BasePath);

            PathProcessing.ValidateBasePath(oldPath);
            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);
            PathProcessing.ValidateDirectoryPath(copyDirectoryName, defaultBasePath: _settings.BasePath, check: false);

            _logger.LogInformation($"Copy directory {oldPath} to {newPath}");

            await Task.Run(() => CopyRecursive(new DirectoryInfo(oldPath), new DirectoryInfo(newPath)), cancelationToken);
        }

        private void CopyRecursive(DirectoryInfo oldPath, DirectoryInfo newPath)
        {
            if (!newPath.Exists)
            {
                newPath.Create();
            }

            foreach (DirectoryInfo dir in oldPath.GetDirectories())
            {
                CopyRecursive(dir, newPath.CreateSubdirectory(dir.Name));
            }
            foreach (FileInfo file in oldPath.GetFiles())
            {
                file.CopyTo(Path.Combine(newPath.FullName, file.Name));
            }
        }
    }
}
