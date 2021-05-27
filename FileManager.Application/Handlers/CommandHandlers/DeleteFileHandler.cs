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
    public class DeleteFileHandler : IDeleteFileHandler
    {
        private readonly ILogger<DeleteDirectoryHandler> _logger;
        private readonly ManagerSettings _settings;

        public DeleteFileHandler(ILogger<DeleteDirectoryHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string deleteFileName = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            deleteFileName = PathProcessing.NormalizaPath(deleteFileName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var basePath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var fullPath = PathProcessing.ComputeFullPath(deleteFileName, basePath);

            PathProcessing.ValidateFilePath(deleteFileName, basePath, _settings.BasePath);

            _logger.LogInformation($"Delete file {fullPath}");

            await Task.Run(() => File.Delete(fullPath), cancelationToken);
        }
    }
}
