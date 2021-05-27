using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Application.Interfaces;
using FileManager.Domain.Models.Enums;
using FileManager.Domain.Models.Manager;
using FileManager.Shared.Constants;
using FileManager.Shared.Extensions;
using FileManager.Shared.Processing;
using FileManager.Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileManager.Application
{
    public class GetItemsHandler : IGetItemsHandler
    {
        private readonly ILogger<GetItemsHandler> _logger;
        private readonly ManagerSettings _settings;

        public GetItemsHandler(ILogger<GetItemsHandler> logger, IOptions<ManagerSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<IEnumerable<Item>> Handle
        (
            string path = PathConstants.BaseDirectorySeparatorChar,
            CancellationToken cancelationToken = default
        )
        {
            path = PathProcessing.NormalizaPath(path);
            var fullPath = PathProcessing.ComputeFullPath(path, _settings.BasePath);
            PathProcessing.ValidateDirectoryPath(path, fullPath, _settings.BasePath);

            _logger.LogInformation($"Listing directory {fullPath}");

            var directoriesTask = Task.Run(() => Directory.EnumerateDirectories(fullPath)
                .Select(e => 
                {
                    var directoryInfo = new DirectoryInfo(e);
                    return (Item)new ManagerDirectory 
                    { 
                        Name = e, 
                        Icon = IconType.ManagerDirectory, 
                        Size = directoryInfo.GetDirectorySize(),
                        Metadata = directoryInfo
                    };
                }), cancelationToken);

            var filesTask = Task.Run(() => Directory.EnumerateFiles(fullPath)
                .Select(e =>
                {
                    var fileInfo = new FileInfo(e);
                    return (Item)new ManagerFile 
                    { 
                        Name = e, 
                        Icon = IconType.ManagerFile,
                        Size = fileInfo.Length,
                        Metadata = fileInfo
                    };
                }), cancelationToken);

            return await new[] { directoriesTask, filesTask }.UnionWhenAll();
        }
    }
}
