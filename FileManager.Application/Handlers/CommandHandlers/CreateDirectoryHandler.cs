using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Application.Interfaces;
using FileManager.Domain.Models.Enums;
using FileManager.Domain.Models.Manager;
using FileManager.Shared.Constants;
using FileManager.Shared.Processing;
using FileManager.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileManager.Application.Handlers.CommandHandlers
{
    public class CreateDirectoryHandler : ICreateDirectoryHandler
    {
        private readonly ILogger<CreateDirectoryHandler> _logger;
        private readonly ManagerSettings _settings;

        public CreateDirectoryHandler(ILogger<CreateDirectoryHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<Item> Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string newDirectoryName = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            newDirectoryName = PathProcessing.NormalizaPath(newDirectoryName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var basePath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var fullPath = PathProcessing.ComputeFullPath(newDirectoryName, basePath);
            
            PathProcessing.ValidateDirectoryPath(newDirectoryName, basePath, _settings.BasePath, false);

            _logger.LogInformation($"Creating directory {fullPath}");

            var createDirectory = await Task.Run(() => Directory.CreateDirectory(fullPath), cancelationToken);

            return
                new ManagerDirectory
                {
                    Name = createDirectory.Name,
                    Icon = IconType.ManagerDirectory,
                    Metadata = createDirectory
                };
        }

    }
}
