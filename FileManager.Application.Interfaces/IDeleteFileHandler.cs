using System.Threading;
using System.Threading.Tasks;
using FileManager.Shared.Constants;

namespace FileManager.Application.Interfaces
{
    public interface IRenameFileHandler
    {
        Task Handle
        (
            string activeDirectory = PathConstants.BaseDirectorySeparatorChar,
            string oldFileName = null,
            string renameFileName = null,
            CancellationToken cancelationToken = default
        );
    }
}
