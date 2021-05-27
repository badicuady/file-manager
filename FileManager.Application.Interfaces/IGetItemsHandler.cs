using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileManager.Domain.Models.Manager;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IGetItemsHandler
    {
        Task<IEnumerable<Item>> Handle(string path = PathConstants.BaseDirectorySeparatorChar, CancellationToken cancelationToken = default);
    }
}
