using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var fullPath = PathProcessing.ValidatePath(PathProcessing.NormalizaPath(path), defaultBasePath: _settings.BasePath);

            _logger.LogInformation($"Listing directory {fullPath}");

            var directoriesTask = Task.Run(() => Directory.EnumerateDirectories(fullPath)
                .Select(e => (Item)new ManagerDirectory { Name = e, Icon = IconType.ManagerDirectory }), cancelationToken);

            var filesTask = Task.Run(() => Directory.EnumerateFiles(fullPath)
                .Select(e => (Item)new ManagerFile { Name = e, Icon = IconType.ManagerFile }), cancelationToken);

            return await new[] { directoriesTask, filesTask }.UnionWhenAll();
        }
    }
}
