using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IDeleteDirectoryHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string deleteDirectoryName = null,
            bool forced = true,
            CancellationToken cancelationToken = default
        );
    }
}
