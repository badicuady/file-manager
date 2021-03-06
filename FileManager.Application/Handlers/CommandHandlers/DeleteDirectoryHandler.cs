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
    public class DeleteDirectoryHandler : IDeleteDirectoryHandler
    {
        private readonly ILogger<DeleteDirectoryHandler> _logger;
        private readonly ManagerSettings _settings;

        public DeleteDirectoryHandler(ILogger<DeleteDirectoryHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string deleteDirectoryName = null,
            bool forced = true,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            deleteDirectoryName = PathProcessing.NormalizaPath(deleteDirectoryName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var basePath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var fullPath = PathProcessing.ComputeFullPath(deleteDirectoryName, basePath);
            
            PathProcessing.ValidateBasePath(fullPath);
            PathProcessing.ValidateDirectoryPath(deleteDirectoryName, basePath, _settings.BasePath);

            _logger.LogInformation($"Delete directory {fullPath}");

            await Task.Run(() => Directory.Delete(fullPath, forced), cancelationToken);
        }
    }
}
