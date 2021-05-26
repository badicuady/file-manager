using System.Threading;
using System.Threading.Tasks;
using FileManager.Domain.Models.Manager;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface ICreateDirectoryHandler
    {
        Task<Item> Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string newDirectoryName = null,
            CancellationToken cancelationToken = default
        );
    }
}
