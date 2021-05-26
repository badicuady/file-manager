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
            var basePath = PathProcessing.ValidatePath(PathProcessing.NormalizaPath(activeDirectory), defaultBasePath: _settings.BasePath);
            var fullPath = PathProcessing.ValidatePath(PathProcessing.NormalizaPath(deleteDirectoryName), basePath, _settings.BasePath);

            _logger.LogInformation($"Delete directory {fullPath}");

            await Task.Run(() => Directory.Delete(fullPath, forced), cancelationToken);
        }
    }
}
