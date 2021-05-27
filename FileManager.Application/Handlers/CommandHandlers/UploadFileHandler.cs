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
    public class UploadFileHandler : IUploadFileHandler
    {
        private readonly ILogger<UploadFileHandler> _logger;
        private readonly ManagerSettings _settings;

        public UploadFileHandler(ILogger<UploadFileHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar, 
            string uploadFileName = null,
            MemoryStream uploadFile = null,
            CancellationToken cancelationToken = default
        )
        {
            activeDirectory = PathProcessing.NormalizaPath(activeDirectory);

            PathProcessing.ValidateDirectoryPath(activeDirectory, defaultBasePath: _settings.BasePath);

            var basePath = PathProcessing.ComputeFullPath(activeDirectory, _settings.BasePath);
            var fullPath = PathProcessing.ComputeFullPath(uploadFileName, basePath);

            _logger.LogInformation($"Upload file {fullPath}");

            await Task.Run(() => File.WriteAllBytes(fullPath, uploadFile.ToArray()), cancelationToken);
        }
    }
}
