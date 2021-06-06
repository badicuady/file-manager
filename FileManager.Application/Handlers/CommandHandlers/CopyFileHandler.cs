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
    public class CopyFileHandler : ICopyFileHandler
    {
        private readonly ILogger<CopyFileHandler> _logger;
        private readonly ManagerSettings _settings;

        public CopyFileHandler(ILogger<CopyFileHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<Item> Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar, 
            string oldFileName = null,
            string copyFileName = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);
            copyFileName = PathProcessing.NormalizaPath(copyFileName);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var basePath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var oldFullPath = PathProcessing.ComputeFullPath(oldFileName, basePath);
            var fullPath = PathProcessing.ComputeFullPath(copyFileName, basePath);

            PathProcessing.ValidateFilePath(oldFileName, basePath, _settings.BasePath);
            PathProcessing.ValidateFilePath(copyFileName, basePath, _settings.BasePath, check: false);

            _logger.LogInformation($"Copy file {oldFullPath} to {fullPath}");

            await Task.Run(() => File.Copy(oldFullPath, fullPath), cancelationToken);

            var fileInfo = new FileInfo(fullPath);
            return new ManagerFile 
            { 
                Name = fullPath, 
                Icon = IconType.ManagerFile,
                Size = fileInfo.Length,
                Metadata = fileInfo
            };
        }
    }
}
