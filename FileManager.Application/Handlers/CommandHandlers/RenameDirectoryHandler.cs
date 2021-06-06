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
    public class RenameDirectoryHandler : IRenameDirectoryHandler
    {

        private readonly ILogger<RenameDirectoryHandler> _logger;
        private readonly ManagerSettings _settings;

        public RenameDirectoryHandler(ILogger<RenameDirectoryHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string renameDirectoryName = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            renameDirectoryName = PathProcessing.NormalizaPath(renameDirectoryName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var oldPath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var newPath = PathProcessing.ComputeFullPath(renameDirectoryName, _settings.BasePath);
           
            PathProcessing.ValidateBasePath(oldPath);
            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);
            PathProcessing.ValidateDirectoryPath(renameDirectoryName, defaultBasePath: _settings.BasePath, check: false);

            _logger.LogInformation($"Rename directory {oldPath} to {newPath}");

            await Task.Run(() => Directory.Move(oldPath, newPath), cancelationToken);
        }
    }
}
