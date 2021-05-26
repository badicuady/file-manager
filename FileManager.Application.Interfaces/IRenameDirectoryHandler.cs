using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IRenameDirectoryHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string renameDirectoryName = null,
            CancellationToken cancelationToken = default
        );
    }
}
