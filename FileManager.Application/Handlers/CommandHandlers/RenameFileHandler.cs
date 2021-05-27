using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Application.Interfaces;
using FileManager.Shared.Constants;
using FileManager.Shared.Processing;
using FileManager.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileManager.Application.Handlers.CommandHandlers
{
    public class RenameFileHandler : IRenameFileHandler
    {
        private readonly ILogger<RenameFileHandler> _logger;
        private readonly ManagerSettings _settings;

        public RenameFileHandler(ILogger<RenameFileHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar, 
            string oldFileName = null,
            string renameFileName = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            renameFileName = PathProcessing.NormalizaPath(renameFileName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var basePath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var oldFullPath = PathProcessing.ComputeFullPath(oldFileName, basePath);
            var fullPath = PathProcessing.ComputeFullPath(renameFileName, basePath);

            PathProcessing.ValidateFilePath(oldFileName, basePath, _settings.BasePath);
            PathProcessing.ValidateFilePath(renameFileName, basePath, _settings.BasePath, check: false);

            _logger.LogInformation($"Rename file {oldFullPath} to {fullPath}");

            await Task.Run(() => File.Move(oldFullPath, fullPath), cancelationToken);
        }
    }
}
